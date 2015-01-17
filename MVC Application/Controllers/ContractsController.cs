using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.DataAccessLayer.MongoDB;
using Monei.Entities;
using Monei.MvcApplication.Models;
using MongoDB.Bson;

namespace Monei.MvcApplication.Controllers
{
	[Authorize]
    public class ContractsController : MoneiControllerBase
    {


        // GET: /Contracts/
        public ActionResult Index()
        {
			//AccountId

			ContractListModel model = new ContractListModel();
			model.Contracts = ContractRepository.Instance.ListContracts(GetAccount().Id);			

            return View("Contracts-Index", model);
        }

        //
        // GET: /Contracts/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Contracts/Create
        public ActionResult Create()
        {
			ContractModel model = new ContractModel();
			model.Contract = null;

            return View(model);
        }

        //
        // POST: /Contracts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

				string supplier = collection["Contract.Supplier"];
				string startDateText = collection["Contract.StartDate"];
				string endDateText = collection["Contract.EndDate"];
				string dueAmountText = collection["Contract.DueAmount"];

				DateTime startDate = DateTime.Parse(startDateText);
				DateTime? endDate = null;
				if(endDateText != "") endDate = DateTime.Parse(endDateText);
				decimal dueAmount = Decimal.Parse(dueAmountText);

				Contract contract = new Contract()
				{
					Account = GetAccount(),
					Supplier = supplier,
					StartDate = startDate,
					EndDate = endDate,
					DueAmount = dueAmount,

				};

				ContractRepository.Instance.CreateContract(contract);


                return RedirectToAction("Index");
            }
            catch(Exception exc)
            {
				logger.Error(exc);
				string errorMessage = "Fail to create the contract. " + exc.Message;
				ViewBag.ErrorMessage = errorMessage;
                return View();
            }
        }

        //
        // GET: /Contracts/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Contracts/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //
        // POST: /Contracts/Delete/5
        [HttpPost]
        public ActionResult Delete( string id /*int id, FormCollection collection*/)
        {
			logger.InfoFormat("Delete the contract with id: " + id);

            try
            {
                // TODO: Add delete logic here

				int contractId = int.Parse(id);

				ContractRepository.Instance.Delete(contractId);

                return RedirectToAction("Index");
            }
            catch(Exception exc)
            {
				logger.ErrorFormat("Fail to delete Contract, Id: \"{0}\". Error:\n{1}", id, exc);
                return View();
            }
        }
    }
}
