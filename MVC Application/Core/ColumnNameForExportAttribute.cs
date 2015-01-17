using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Core
{
	public class ColumnNameForExportAttribute : Attribute
	{
		public string ColumnName { get; private set; }

		public ColumnNameForExportAttribute(string columnName)
		{
			ColumnName = columnName;
		}
	}
}