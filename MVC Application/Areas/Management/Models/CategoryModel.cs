using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monei.Entities;

namespace Monei.MvcApplication.Areas.Management.Models
{
	public class CategoryListModel
	{
		public IEnumerable<Category> Categories { get; set; }

	}

	//public class CategoryEditSaveModel
	//{
	//	public Category Category { get; set; }
	//}

}