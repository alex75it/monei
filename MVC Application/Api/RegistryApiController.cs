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

        #region private
        private ExcelPackage CreateExcel(IEnumerable<RegistryRecord> records)
        {
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Export");

            sheet.Cells["A1"].Value = "aaa";
            return package;
        }

        public int Create(RegistryRecord record)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}