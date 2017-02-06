using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Monei.MvcApplication.Api.PostDataObjects;
using OfficeOpenXml;
using Monei.MvcApplication.Filters;

namespace Monei.MvcApplication.Api
{
    [RoutePrefix("api/registry")]
    public class RegistryApiController : ApiControllerBase
    {
        [HttpPost, Route("search")]
        public IEnumerable<RegistryRecord> Search(RegistrySearchPostData data)
        {
            RegistryFilters filters = new RegistryFilters();
            filters.StartDate = data.FromDate;
            filters.EndDate = data.ToDate;
            filters.Categories = data.Categories;

            var records = RegistryRepository.ListRecords(filters);

            // HACK: remove subitems to prevent LazyInitializationException 
            foreach (var record in records)
            {
                //if (record.Category != null)
                //    record.Category.Subcategories = null;
                if (record.Subcategory != null)
                    record.Subcategory.Category = null;
            }

            return records;
        }

        [ApiAuthorizationFilter]
        [HttpPost, Route("")]
        public int Create(RegistryNewRecordPostData postData)
        {
            if (postData.Date == DateTime.MinValue) throw new Exception("Date is not defined");
            if (postData.Amount == 0) throw new Exception("Amount is zero");
            if (postData.CategoryId == 0) throw new Exception("Category is not defined");

            var record = new RegistryRecord() {
                CreationAccount = base.CurrentAccount,
                Account = base.CurrentAccount, // currently is not possible to set data for another account
                Date = postData.Date,
                OperationType = postData.Operation,
                Amount = postData.Amount,
                Category = new Category() { Id = postData.CategoryId },
                Subcategory = postData.SubcategoryId == 0 ? null : new Subcategory() { Id = postData.SubcategoryId },
                Note = postData.Note,
                IsTaxDeductible = postData.IsTaxDeductible,
                IsSpecialEvent = postData.IsSpecialEvent,
            };

            int newId = RegistryRepository.Create(record);
            return newId;
        }

        #region private

        private ExcelPackage CreateExcel(IEnumerable<RegistryRecord> records)
        {
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Export");

            sheet.Cells["A1"].Value = "aaa";
            return package;
        }

        #endregion
    }
}