using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Api.PostDataObjects
{
	public class SubcategoryPostData
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}

	//public class SubcategoryPostDataConverter : TypeConverter
	//{ 
	
	//}
}