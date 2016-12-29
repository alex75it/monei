using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Monei.MvcApplication.Api.PostDataObjects;
using OfficeOpenXml;

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

            var list = RegistryRepository.ListRecords(filters);
            return list;
        }

        [HttpPost, Route("")]
        public int Create(RegistryNewRecordPostData postData)
        {
            var record = new RegistryRecord() {
                CreationAccount = base.CurrentAccount,
                Account = base.CurrentAccount, // temporary unsettable
                Date = postData.Date,
                OperationType = postData.OperationType,
                Amount = postData.Amount,
                Category = new Category() { Id = postData.CategoryId },
                Subcategory = new Subcategory() { Id = postData.SubcategoryId },  
                Note = postData.Note,
                IsTaxDeductible = postData.IsTaxDeductible,
                IsSpecialEvent = postData.IsSpecialEvent,
            };

            record = RegistryRepository.AddRecord(record);
            return record.Id;
        }

        public RegistryRecord CreateRecord(RegistryNewRecordPostData postData)
        {
            throw new NotImplementedException();
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