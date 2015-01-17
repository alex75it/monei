using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.Core.DataAnalysis.BusinessObjects
{
	public class CategoryData
	{

		//public static Dictionary<int, string> Categories {get; set;}

		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }


		public CategoryData(Category category, decimal amount) { 
			Id = category.Id;
			Name = category.Name;
			Amount = amount;
		}

	}
}
