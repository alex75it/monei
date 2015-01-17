using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Controllers.Api.PostDataObjects
{
	public class MoveSubcategoryPostData
	{
		public int SubcategoryId { get; set; }
		public int CategoryId { get; set; }
	}
}