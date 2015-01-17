using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Monei.Core.DataAnalysis.BusinessObjects
{
	public class MonthData
	{
		[JsonProperty("Index")]
		public int Month { get; set; }
		public decimal Income { get; set; }
		public decimal Outcome { get; set; }
		public decimal Result { get; set; }
		
		public CategoriesData Categories { get; set; }

		public MonthData(int month)
		{
			Month = month;
			Categories = new CategoriesData();
		}		

	}
}
