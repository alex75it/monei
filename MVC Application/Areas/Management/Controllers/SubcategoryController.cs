using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Areas.Management.Models;
using Monei.MvcApplication.Controllers;
using Monei.Core.BusinessLogic;

namespace Monei.MvcApplication.Areas.Management.Controllers
{
    public class SubcategoryController : MoneiControllerBase
    {
        public ICategoryRepository CategoryRepository { get; set; }
        public ISubcategoryRepository SubcategoryRepository { get; set; }

        public SubcategoryController(ISecurityManager securityManager) :base(securityManager)
        {

        }

        // GET: /Management/Subcategory/
        public ActionResult Index()
        {
            if (!IsAuthorized(Account.AccountRole.Administrator))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            ClearReturnUrl();

            SubcategoryListModel model = new SubcategoryListModel();
            if (ViewBag.CategoryId != null)
                model.SelectedCategoryId = ViewBag.CategoryId;

            //model.Subcategories = SubcategoryRepository.List();
            model.Categories = CategoryRepository.List();

            return View("Subcategory-Index", model);
        }

        [HttpGet]
        public ActionResult Create(string categoryId)
        {
            int? categoryIdInt = null;
            if(!string.IsNullOrWhiteSpace(categoryId))
                categoryIdInt = int.Parse(categoryId);

            SubcategoryCreateModel model = new SubcategoryCreateModel();
            model.CategoryId = categoryIdInt;

            return View("Subcategory-Create", model);
        }

        [HttpPost]
        public ActionResult Create(SubcategoryCreateModel model, FormCollection collection)
        {
            int categoryId = collection.GetIntValue("categoryId");

            Category category = CategoryRepository.Read(categoryId);

            Subcategory subcategory = new Subcategory() { 
                Category = category,
                Name = model.Name,
                Description = model.Description
            };

            SubcategoryRepository.Create(subcategory);

            ViewBag.CategoryId = categoryId;
            return View("Subcategory-Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int subcategoryId = int.Parse(id);
                        
            Subcategory subcategory = SubcategoryRepository.Read(subcategoryId);

            SubcategoryEditModel model = new SubcategoryEditModel() { 
                Id = subcategory.Id,
                Name = subcategory.Name,
                Description = subcategory.Description,
                CategoryId = subcategory.Category.Id
            };

            model.CategoryList = CategoryRepository.List().Select(i => new SelectListItem() {  Value=i.Id.ToString(), Text=i.Name, Selected=i.Id == subcategory.Category.Id}).ToList();

            return View("Subcategory-Edit", model);
        }


        [HttpPost]
        public ActionResult Edit(SubcategoryEditModel model)
        {
            Subcategory subcategory = SubcategoryRepository.Read(model.Id);
            subcategory.Name = model.Name;
            subcategory.Description = model.Description;
            subcategory.Category = CategoryRepository.Read(model.CategoryId);
                        
            try
            {
                SubcategoryRepository.Update(subcategory);
            }
            catch(Exception exc)
            {
                Guid errorId = Guid.NewGuid();
                logger.Error(string.Format("Fail to update Subcategory. Error ID: {0}.", errorId), exc);
                SetErrorMessage("Fail to update Subcategory. Error ID: " + errorId + ".");

                model.CategoryList = CategoryRepository.List().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name, Selected = i.Id == subcategory.Category.Id }).ToList();
                return View("Subcategory-Edit", model);
            }

            SetSuccessMessage(string.Format("Subcategory \"{0}\" updated", subcategory.Name));
            ViewBag.CategoryId = model.CategoryId;
            return View("Subcategory-Index");
        }

        [HttpGet]
        public ActionResult PartialCreate()
        {
            // Partial View cannot be used because it cannot use @section
            return PartialView("PartialSubcategory-Create");
        }

        [HttpPost]
        public ActionResult PartialCreate(FormCollection collection)
        {
            int categoryId = collection.GetIntValue("categoryId");


            return PartialView("PartialSubcategory-Create");
        }


        [HttpGet]
        public virtual ActionResult GetSubcategories(string categoryId)
        {
            int categoryIdInt = int.Parse(categoryId);
            var subcategories = (from subcategory in SubcategoryRepository.List(categoryIdInt)
                                 select new { Id = subcategory.Id, Name = subcategory.Name, Description=subcategory.Description }
                ).ToList();			
            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }		


    }
}
