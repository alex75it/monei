using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.DataAccessLayer.Filters;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Helpers;
using Monei.MvcApplication.Models;
using MongoDB.Bson;
using Monei.MvcApplication;
using Monei.Entities.Interfaces;


namespace Monei.MvcApplication.Controllers
{
    //[ActionLinkArea]
    [Authorize]
    public class RegistryController : MoneiControllerBase
    {

        private const string SESSION_FILTERS = "RegistryFilters";

        public ICategoryRepository CategoryRepository { get; set; }
        public IRegistryRepository RegistryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }
 
        public ActionResult Index()
        {		
            RegistryFilters filters = null;

            // reload filters if it is a return from here 
            if(Request.QueryString["from"] == "Registry")
                filters = Session[SESSION_FILTERS] as RegistryFilters;
            
            RegistryListModel model = new RegistryListModel();

            if(filters == null) {
                // create default filters
                filters = new RegistryFilters()
                {
                    AccountId = GetAccount().Id,
                    StartDate = Utils.GetFirstDayOfMonth(),
                    EndDate = Utils.GetLastDayOfMonth(),
                    SelectedPeriod = RegistryFilters.PERIOD_CURRENT_MONTH,
                    CategoryId = null,
                };
            }

            model.Filters = filters;	

            model.CategorySelectList = new SelectList(CategoryRepository.List(), "Id", "Name",  filters.CategoryId);
            if (IsAuthorized(Account.AccountRole.Administrator))
                model.AccountSelectList = new SelectList(AccountRepository.ListAll(), "Id", "Username");
            
            model.Records = RegistryRepository.ListRecods(filters).OrderByDescending(r => r.Date).ToList();
            
            return View("Registry-Index", model);
        }


        [HttpGet]
        public ActionResult Search()
        {
            return RedirectToAction("Index");
        }

        // POST: /Registry/Search
        public ActionResult Search(FormCollection formCollection)
        {
            //logger.Debug("Search()");

            RegistryFilters filters = new RegistryFilters();

            filters.AccountId = GetAccount().Id;

            DateTime startDate;
            if (!DateTime.TryParse(formCollection.GetStringValue("StartDate"), out startDate))
                throw new Exception("Insert a correct date");

            filters.StartDate = startDate;
            
            string endDateValue = formCollection.GetStringValue("EndDate");
            if (!string.IsNullOrWhiteSpace(endDateValue))
            {
                DateTime endDate;
                if (!DateTime.TryParse(endDateValue, out endDate))
                    throw new Exception(string.Format("Parsing of EndDate field value fail, value: \"{0}\".", endDateValue));
                filters.EndDate = endDate;
            }

            // set the selected perdio (if exists)
            filters.SelectedPeriod = formCollection.GetStringValue("selectedPeriod");

            filters.CategoryId = formCollection.GetIntValue("Category", null);
            filters.SubcategoryIds = formCollection.GetIntValues("Subcategory");

            filters.TextToSearch = formCollection.GetStringValue("TextToSearch", null);

			
			filters.Amount = (decimal)formCollection.GetDecimalValueIfPassed("AmountToSearch", 0m);
			
            filters.SetOperationType(OperationType.Transfer, formCollection.ContainsKey("IncludeTransfers"));

            filters.ShowOnlyTaxDeductible = formCollection.ContainsKey("ShowOnlyTaxDeductible");
            filters.IncludeSpecialEvent = formCollection.ContainsKey("IncludeSpecialEvent");

            if (formCollection.ContainsKey("Account"))
                filters.AccountId = formCollection.GetIntValue("Account", null);
                
            filters.Normalize();

            RegistryListModel model = new RegistryListModel();
            model.Filters = filters;

            model.CategorySelectList = new SelectList(CategoryRepository.List(), "Id", "Name", model.Filters.CategoryId);
            

            if (IsAuthorized(Account.AccountRole.Administrator))
                model.AccountSelectList = new SelectList(AccountRepository.ListAll(), "Id", "Username", model.Filters.AccountId);
            
            // store filters in Session
            Session.Add(SESSION_FILTERS, filters);

            var list = RegistryRepository.ListRecods(filters);
            
            // it is a request of Export?
            string export = formCollection.GetStringValue("export");
            if (export == "csv")
            {
                var csvList = (ICollection<ICsvFormattable>)list.OrderBy(r => r.Date).Cast<ICsvFormattable>().ToList();
                string fileName = string.Format("Registry {0} {1}.csv", 
                    filters.StartDate.ToString("yyyy-MM-dd"), 
                    filters.EndDate.ToString("yyyy-MM-dd"));
                return new ExportResponseCreator().CreateResult(Response, csvList, fileName);
            }

            model.Records = list.OrderByDescending(r => r.Date).ToList();
            return View("Registry-Index", model);
        }

        // POST: /Registry/List
        public ActionResult List(DateTime fromDate, DateTime toDate, int categoryId)
        {
            logger.InfoFormat("List(from, to, category)");

            RegistryListModel model = new RegistryListModel();

            var filters = new RegistryFilters() {AccountId = GetAccount().Id, CategoryId = categoryId, StartDate=fromDate, EndDate=toDate };
            var list = RegistryRepository.ListRecods(filters).OrderByDescending(r => r.Date).ToList();

            model.Records = list;

            return View(model);
        }


        // GET: /Registry/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /Registry/Create
        public ActionResult Create()
        {            
            RegistryCreateRecordModel model = new RegistryCreateRecordModel();

            model.Record = new RegistryRecord() { Date = DateTime.Today };
            return View("Registry-Create", model);
        }

        //
        // POST: /Registry/Create

        [HttpPost]
        //public ActionResult Create(NewRegistryRecordModel model)
        public ActionResult Create(RegistryCreateRecordModel model, FormCollection collection)
        {
            //if(ModelState.IsValid)
            {
                try
                {
                    Account account = GetAccount();
    
                    int categoryId = collection.GetIntValue("Category.Id");
                    int? subcategoryId = collection.GetIntValue("Subcategory.Id");
                    decimal amount = Math.Abs(model.Record.Amount);
            
                    Category category = CategoryRepository.Read(categoryId);
                    Subcategory subcategory = null;
                    if (subcategoryId.HasValue)
                        subcategory = SubcategoryRepository.Read(subcategoryId.Value);

                    OperationType operationType = (OperationType)collection.GetIntValue("operationType");

                    RegistryRecord item = new RegistryRecord() {
                        Account = account,
                        Date = model.Record.Date,						
                        Amount = amount,
                        OperationType = operationType,
                        Category = category,
                        Subcategory = subcategory,
                        Note = model.Record.Note,
                        IsTaxDeductible = model.Record.IsTaxDeductible,
                        IsSpecialEvent = model.Record.IsSpecialEvent,
                    };

                    item = RegistryRepository.AddRecord(item);

                    // add another?
                    if (collection.GetStringValue("save another") == "True")
                    {
                        ViewBag.Message = "Record add to the Registry";
                        ViewBag.Clear = true;					
                        return Create();
                    }

                    return RedirectToAction("Index");
                }
                catch(Exception exc)
                {
                    logger.ErrorFormat("Fail to save record in registry.\n{0}", exc);

                    ModelState.AddModelError("CreateError", "Fail to create record");
                    return Create(); // View(model);
                }
            }

        }
        
        
        // GET: /Registry/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                RegistryRecord record = RegistryRepository.Read(id);

                RegistryCreateRecordModel model = new RegistryCreateRecordModel()
                {
                    Record = record,
                //	SelectedCategoryId = record.Category.Id,
                };

                return PartialView("Registry-Edit-Partial", model);
            }
            catch (Exception exc)
            {
                Guid errorId = Guid.NewGuid();
                logger.ErrorFormat("Error on show Rgistry record edit form. Error ID: {0}.\r\n{1}", errorId, exc);
                base.SetErrorMessage("Some error occour. Error ID: " + errorId);
                return View("Registry-Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(RegistryCreateRecordModel model, FormCollection collection)
        {
            //System.Diagnostics.Contracts.Contract.Requires<InvalidArgumentException>(model.Record.Id > 0);

            //if(ModelState.IsValid)
            {
                try
                {
                    int recordId = model.Record.Id;

                    Account account = GetAccount();

                    int categoryId = collection.GetIntValue("Category.Id");
                    int? subcategoryId = collection.GetIntValue("Subcategory.Id");
                    decimal amount = Math.Abs(model.Record.Amount);

                    Category category = CategoryRepository.Read(categoryId);
                    Subcategory subcategory = null;
                    if (subcategoryId.HasValue)
                        subcategory = SubcategoryRepository.Read(subcategoryId.Value);

                    OperationType operationType = (OperationType)collection.GetIntValue("operationType");

                    RegistryRecord item = RegistryRepository.Read(recordId);

                    // todo: this will do in business logic class
                    if(item == null)
                        throw new Exception("Fail to load record with id=" + recordId);

                    item.Date = model.Record.Date;
                    item.Amount = amount;
                    item.OperationType = operationType;
                    item.Category = category;
                    item.Subcategory = subcategory;
                    item.Note = model.Record.Note;
                    item.IsTaxDeductible = model.Record.IsTaxDeductible;
                    item.IsSpecialEvent = model.Record.IsSpecialEvent;
                    
                    // todo: this will do in business logic class
                    item.LastChangeDate = DateTime.UtcNow;
                    item.LastUpdateAccount = account;

                    RegistryRepository.UpdateRecord(item);

                    SetSuccessMessage("Registry record updated");
                    //return Edit(model.Record.Id);
                    //return View(model);
                    return Content("ok");
                }
                catch (Exception exc)
                {
                    logger.ErrorFormat("Fail to update record in registry.\n{0}", exc);

                    SetErrorMessage("Fail to create record");
                    //return Edit(model.Record.Id);
                    return Content("error");
                }
            }

        }


        //// POST: /Registry/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //	try
        //	{
        //		// TODO: Add update logic here

        //		return RedirectToAction("Index");
        //	}
        //	catch
        //	{
        //		return View();
        //	}
        //}


        // POST: /Registry/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            logger.InfoFormat("Delete record with id: {0}.", id);

            try
            {
                RegistryRepository.DeleteRecord(int.Parse(id));

                return RedirectToAction("Index", new {from="Registry"});
            }
            catch(Exception exc)
            {
                logger.ErrorFormat("Fail to delete Registry record, Id: \"{0}\". Error:\n{1}", id, exc);
                // todo: show error
                return View();
            }
        }


        [HttpGet]
        public virtual ActionResult GetCategories()
        {
            var categories = ( 
                from category in CategoryRepository.List()
                select new { Id=category.Id, Name=category.Name}
                )
                .ToList();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult GetSubcategories(string categoryId)
        {
            int categoryIdInt = int.Parse(categoryId);
            var subcategories = (from subcategory in SubcategoryRepository.List(categoryIdInt)
                                 select new { Id=subcategory.Id, Name=subcategory.Name}
                ).ToList();
            subcategories.Insert(0, new { Id = UNDEFINED_ID, Name = "(undefined)" });
            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }


        //private int? GetIntFromString(string stringValue)
        //{
        //	int value = -1;
        //	if (!int.TryParse(stringValue, out value))
        //		return null;
        //	return value;
        //}

    }//class
}
