using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using Newtonsoft.Json;

namespace Monei.Core.DataAnalysis.BusinessObjects
{
	//[JsonProperty("Categories")]
	public class CategoriesData
	{
		[JsonIgnore]
		public SortedDictionary<int, CategoryData> List { get; set; }
		
		public List<CategoryData> Categories { get { return List.Select(kv => kv.Value).ToList(); } }

		public CategoriesData() {
			List = new SortedDictionary<int, CategoryData>();
		}

		public void AddValue(int categoryId, CategoryData data)
		{
			List.Add(categoryId, data);
		}

		public void AddNewCategories(IEnumerable<Category> categories) { 
			foreach(var category in categories)
			{
				if(!List.ContainsKey(category.Id))
					List.Add(category.Id, new CategoryData(category, 0m));

			}
		}

	}
}
