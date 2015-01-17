using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.Entities;

namespace Monei.MvcApplication.Areas.Management.Models
{
	public class SubcategoryListModel
	{
		public IEnumerable<Category> Categories { get; set; }
		public int? SelectedCategoryId { get; set; }
		public IEnumerable<Subcategory> Subcategories { get; set; }

		//public 

	}


	public class SubcategoryCreateModel
	{
		public int? CategoryId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
	}

	public class SubcategoryEditModel
	{
		//Subcategory Subcategory { get; set;  }
		public int Id { get; set; }
		public int CategoryId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }

		public IEnumerable<SelectListItem> CategoryList { get; set; }
	}

}