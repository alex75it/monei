using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.Core.Managers;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Areas.Management.Models;
using Monei.MvcApplication.Controllers;


namespace Monei.MvcApplication.Areas.Management.Controllers
{
	public class CategoryController :MoneiControllerBase
	{

		public ICategoryRepository CategoryRepository { get; set; }

		// GET: /Management/Category/
		public ActionResult Index()
		{
			if(!IsAuthorized(Account.AccountRole.Administrator))
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

			//if (GetAccount().Role != Account.AccountRole.Administrator)
			//	return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
			// todo: Custom 403 page
			//http://thechefprogrammer.blogspot.it/2013/04/returning-http-403-forbidden-error-with.html
				//return new HttpForbiddenResult("Acces to this page requires \"Administrator\" role."); //
			//HttpUnauthorizedResult("Acces to this page requires \"Administrator\" role.");// Redirect()
						

			ClearReturnUrl();

			CategoryListModel model = new CategoryListModel();

			IEnumerable<Category> list;

			try
			{
				//model.Categories = CategoryRepository.Instance.List();
				list = CategoryRepository.List();
			}
			catch (Exception exc)
			{ 
				logger.ErrorFormat("Fail to load categories.\r\n" + exc.ToString());
				throw new Exception("Fail to load categories", exc);
			}

			if (TempData.ContainsKey("error"))
				SetErrorMessage(TempData["error"] as string);
			return View("Category-Index", list);
		}

		public ActionResult Management()
		{
			return View("Category-Management");
		}


		// GET: /Management/Category/Create
		public ActionResult Create(string returnUrl)
		{
			SetReturnUrl(returnUrl);

			ViewBag.ReturnUrl = Request.UrlReferrer;
			return View("Category-Create");
		}

		// POST: /Management/Category/Create
		[HttpPost]
		public ActionResult Create(Category model)
		{
			if (ModelState.IsValid)
			{			
				try
				{
					Account currentAccount = GetAccount();
					model.CreationAccount = currentAccount;
					CategoryRepository.Create(model);
				}
				catch (Exception exc)
				{
					Guid errorId = Guid.NewGuid();
					logger.ErrorFormat("Fail to create Category (Error ID: {0}). \r\n{1}", errorId, exc);
					ViewBag.ErrorMessage = string.Format("Fail to create Category. (Error ID: {0})", errorId);
					return View("Category-Create", model);
				}

				string returnUrl = GetReturnUrl();
				if (returnUrl != null)
					return Redirect(returnUrl);
				else
					return RedirectToAction("Index");
			}
			else
			{
				return View("Category-Create", model);
			}
		}

		//[HttpPost]
		public ActionResult Edit(string id)
		{
			logger.InfoFormat("Edit record with id: {0}", id);

			try
			{
				int categoryId;
				if (!int.TryParse(id, out categoryId))
					throw new Exception("Fail to obtain ObjectId from \"id\" parameter.");

				Category category = CategoryRepository.Read(categoryId);
				object model = category;
				return View("Category-Edit", model);

				//return RedirectToAction("Index");
			}
			catch (Exception exc)
			{
				Guid errorId = Guid.NewGuid();
				logger.ErrorFormat("Fail to edit Category with id: {0}. Error ID: {0} Error:\n{1}", id, errorId, exc);
				return RedirectToAction("Index");
				//return View("Category-Index");
			}
		}

		[HttpPost]
		public ActionResult Edit(Category category, FormCollection collection)
		{
			//Category category;
			if (!ModelState.IsValid)
			{ 
				foreach(var key in ModelState.Keys)
				{
					if(ModelState[key].Errors.Count > 0)
						logger.ErrorFormat("{0}: {1}", key, ModelState[key].Value);
				}
			}

			try
			{
				category.Id = int.Parse(collection["id"]);
				Account currentAccount = GetAccount();
				category.CreationAccount = currentAccount;
				CategoryManager categoryManager = new CategoryManager(CategoryRepository);
				categoryManager.Update(category);

				return RedirectToAction("Index");
			}
			catch (CategoryTooLongNameException exc)
			{				
				ViewBag.ErrorMessage = string.Format("Name is too long, must be at most {0} characters long.", exc.MaxLength);
				//logger.ErrorFormat("Fail to edit Category with id: {0}. Error ID: {0} Error:\n{1}", id, errorId, exc);				
			}
			catch (Exception exc)
			{
				Guid errorId = Guid.NewGuid();
				logger.ErrorFormat("Fail to update Category with id: {0}. Error ID: {1}. Error:\r\n{2}", category.Id, errorId, exc);
				SetErrorMessage(string.Format("Fail to edit Category. (Error ID: {0})", errorId));
			}

			return View("Category-Edit", category);
		}


		// POST: /Management/Category/Delete/5
		[HttpPost]
		public ActionResult Delete(string id)
		{
			logger.InfoFormat("Delete record with id: {0}.", id);

			try
			{
				int categoryId = int.Parse(id); 
				CategoryRepository.Delete(categoryId);
				return RedirectToAction("Index");
			}
			catch (Exception exc)
			{
				Guid errorId = Guid.NewGuid();
				logger.ErrorFormat("Fail to delete Category record, Id: \"{0}\". Error ID: {1}. Error:\r\n{2}", id,  errorId, exc);
				SetErrorMessage("Fail to delete category. " + exc.Message);
				TempData.Add("error", "Fail to delete category. " + exc.Message);
				return RedirectToAction("Index");
			}
		}


	}//class
}
