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
		

		//[HttpPost, Route("list")]
		//public IEnumerable<RegistryRecord> Search(SearchPostData data)
		//{
		//	IEnumerable<RegistryRecord> records = GetRecords(data);
		//	foreach (var r in records)
		//	{
		//		r.Category.Subcategories = null;  // error on serialization to JSON
		//		r.Subcategory = null;

		//	}
		//	return records;
		//}	




		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public void Post([FromBody]string value)
		{
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
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