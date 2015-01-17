using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using Monei.Entities.Interfaces;

namespace Monei.MvcApplication.Core
{
	public class CsvFormatter :BufferedMediaTypeFormatter
	{
		static char[] specialChars = new char[] { ',', '\n', '\r', '"' };

		public CsvFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
			//SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.ms-excel"));
		}

		public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
		{
			base.SetDefaultContentHeaders(type, headers, mediaType);
			if (headers.ContentDisposition == null || string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName))
			{
				string filename = "money Export.csv";
				headers.Add("Content-Disposition", "attachment; filename=" + filename);
			}
		}

		public override bool CanWriteType(System.Type type)
		{
			return type is ICsvFormattable;
		}

		public override bool CanReadType(Type type)
		{
			return false;
		}

		public override void WriteToStream(Type type, object value, Stream stream, System.Net.Http.HttpContent content)
		{
			using (var writer = new StreamWriter(stream))
			{
				var items = value as IEnumerable<dynamic>;
				if (items != null)
				{
					if (items.Count() > 1)
					{
						object obj = items.First();
						PropertyInfo[] properties = obj.GetType().GetProperties(/*BindingFlags.Public | BindingFlags.GetProperty*/);

						WriteHeaders(properties, writer);

						foreach (var item in items)
						{
							WriteItem(item, properties, writer);
						}
					}
				}
				else
				{
					// Elemento singolo
					if (value == null)
					{
						throw new InvalidOperationException("Cannot serialize type");
					}
					WriteItem(value, writer);
				}
			}
			stream.Close();
		}


		private void WriteItem(dynamic item, StreamWriter writer)
		{
			// TODO: EMA: Questo è un modo ma è difficile generalizzare per tutti i tipi di risposta
			// Quindi meglio fare una factory di metodi per serializzare in base al tipo e 
			// tipizzare tutte le response cosi possiamo decidere con precisione cosa scrivere nel file csv
			// 
			PropertyInfo[] props = item.GetType().GetProperties();

			foreach (var prop in props)
			{
				object value = prop.GetValue(item);
				writer.Write("{0};", Escape(value));
			}
			writer.WriteLine("");
		}

		private void WriteHeaders(PropertyInfo[] properties, StreamWriter writer)
		{
			foreach (PropertyInfo property in properties)
			{
				string columnName = property.GetCustomAttribute<ColumnNameForExportAttribute>().ColumnName;
				writer.Write("{0};", Escape(columnName));
			}
			writer.WriteLine("");
		}

		private void WriteItem(dynamic item, PropertyInfo[] properties, StreamWriter writer)
		{
			foreach (var property in properties)
			{
				object value = property.GetValue(item);
				writer.Write("{0};", Escape(value));
			}
			writer.WriteLine("");
		}		

		private string Escape(object o)
		{
			if (o == null)
			{
				return "";
			}

			string value = o.ToString();
			if (value.IndexOfAny(specialChars) != -1)
			{
				return String.Format("\"{0}\"", value.Replace("\"", "\"\""));
			}
			else return value;
		}
	}
}