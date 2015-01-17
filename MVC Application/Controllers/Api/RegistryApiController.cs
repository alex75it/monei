using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using OfficeOpenXml;

namespace Monei.MvcApplication.Controllers.Api
{
	
	[RoutePrefix("api/registry")]
    public class RegistryApiController : ApiControllerBase
    {

		[HttpPost, Route("list")]
		public IEnumerable<RegistryRecord> Search(SearchPostData data)
		{
			IEnumerable<RegistryRecord> records = GetRecords(data);
			foreach (var r in records)
			{
				r.Category.Subcategories = null;  // error on serialization to JSON
				r.Subcategory = null;
				
			}
			return records;
		}		

		[HttpPost, Route("export")]
		public HttpResponseMessage Export(SearchPostData data)
		{
			string fileName = "monei Export.xlsx";


			IEnumerable<RegistryRecord> records = GetRecords(data);

			ExcelPackage excel = CreateExcel(records);

			HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);


			result.Content = new StringContent("aaaaaaaaaaaaaaa");
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"); //attachment will force download
			result.Content.Headers.ContentDisposition.FileName = fileName;

			return result;
		}



		public class SearchPostData {
			public string StartDate { get; set; }
			public string EndDate { get; set; }
		}

		private IEnumerable<RegistryRecord> GetRecords(SearchPostData data)
		{
			RegistryFilters filters = new RegistryFilters()
			{
				StartDate = DateTime.ParseExact(data.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
				EndDate = DateTime.ParseExact(data.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
			};
			IEnumerable<RegistryRecord> records = RegistryRepository.ListRecods(filters);
			return records;
		}

		private ExcelPackage CreateExcel(IEnumerable<RegistryRecord> records)
		{
			ExcelPackage package = new ExcelPackage();
			ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Export");

			sheet.Cells["A1"].Value = "aaa";
			return package;
		}
    }
}
