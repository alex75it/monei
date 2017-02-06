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
using Monei.MvcApplication;
using Monei.Entities.Interfaces;
using Monei.Core.BusinessLogic;

namespace Monei.MvcApplication.Controllers
{
    [Authorize]
    public class RegistryController : MoneiControllerBase
    {

        private const string SESSION_FILTERS = "RegistryFilters";

        public ICategoryRepository CategoryRepository { get; set; }
        public IRegistryRepository RegistryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }

        public RegistryController(ISecurityManager securityManager) : base(securityManager)
        {

        }

        public ActionResult Index()
        {		
            return View("Registry-Index");
        }


        [HttpGet]
        public ActionResult Search()
        {
            return RedirectToAction("Index");
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

                RegistryRepository.Create(item);

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
    }
}
