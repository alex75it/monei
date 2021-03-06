﻿using System;
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

            var records = RegistryRepository.ListRecords(filters);

            // HACK: remove subitems to prevent LazyInitializationException 
            foreach (var record in records)
            {
                if (record.Category != null)
                    record.Category.Subcategories = null;
                if (record.Subcategory != null)
                    record.Subcategory.Category = null;
            }

            return records;
        }
        public int Create(RegistryRecord record)
        {
            record = RegistryRepository.AddRecord(record);
            return record.Id;
        }

        [HttpPost, Route("")]
        public int Create(RegistryNewRecordPostData postData)
        {
            var record = new RegistryRecord() {
                CreationAccount = base.CurrentAccount,
                Account = base.CurrentAccount, // temporary unsettable
                Date = postData.Date,
                Category = new Category() { Id = postData.CategoryId },
                Subcategory = new Subcategory() { Id = postData.SubcategoryId },                
                Amount = postData.Amount,                                
                Note = postData.Note
            };

            record = RegistryRepository.AddRecord(record);
            return record.Id;
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