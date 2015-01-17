using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Monei.Core.DataAnalysis.BusinessObjects
{
	public class YearData
	{
		public int Year { get; set; }
		public decimal Income { get; set; }
		public decimal Outcome { get; set; }
		public decimal Result { get; set; }

		[JsonProperty("Categories")]
		public CategoriesData Categories { get; set; }

		public IList<MonthData> Months {get; set;}

		public YearData(int year)
		{
			Year = year;
			Months = new List<MonthData>();
		}	
		
		

	}
}
