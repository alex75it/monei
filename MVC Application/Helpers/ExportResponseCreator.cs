using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Monei.Entities.Interfaces;
using Monei.MvcApplication.Core;

namespace Monei.MvcApplication.Helpers
{
	public class ExportResponseCreator
	{
		static char[] specialChars = new char[] { ',', '\n', '\r', '"' };

		public ContentResult CreateResult(HttpResponseBase response, ICollection<ICsvFormattable> items, string fileName)
		{
			response.ClearHeaders();
			response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
			response.AppendHeader("Content-Type", "application/vnd.ms-excel; charset=utf-8");
			response.Expires = 0; //no cache

			ContentResult result = new ContentResult() { 
				//ContentEncoding = ""
				ContentType = "text/csv",
				Content = CreateCsv(items),
			};

			return result;
		}

		private string CreateCsv(ICollection<ICsvFormattable> items)
		{
			StringBuilder content = new StringBuilder();			

			if (items.Count() > 1)
			{
				content.AppendLine(items.First().CreateHeadersRow());

				foreach (var item in items)
				{
					content.AppendLine(item.CreateRow());
				}
			}

			return content.ToString();
		}


		private string CreateCsv(ICollection<object> items)
		{
			//using (var writer = new StreamWriter(stream))
			StringBuilder content = new StringBuilder();

			if (items.Count() > 1)
			{
				object obj = items.First();
				PropertyInfo[] properties = obj.GetType().GetProperties(/*BindingFlags.Public | BindingFlags.GetProperty*/);

				WriteHeaders(properties, content);

				foreach (var item in items)
				{
					WriteItem(item, properties, content);
				}
			}
			
			return content.ToString();
		}

		private void WriteHeaders(PropertyInfo[] properties, StringBuilder content)
		{
			foreach (PropertyInfo property in properties)
			{
				string columnName = property.GetCustomAttribute<ColumnNameForExportAttribute>().ColumnName;
				content.AppendFormat("{0};", Escape(columnName));
			}
			content.Append("\r\n");
		}

		private void WriteItem(dynamic item, PropertyInfo[] properties, StringBuilder content)
		{
			foreach (var property in properties)
			{
				object value = property.GetValue(item);
				content.AppendFormat("{0};", Escape(value));
			}
			content.Append("\r\n");
		}		

		
		private string Escape(object o)
		{			
			if (o == null)
				return "";

			string value = o.ToString();
			if (value.IndexOfAny(specialChars) != -1)
				return String.Format("\"{0}\"", value.Replace("\"", "\"\""));
			
			return value;
		}



	}
}