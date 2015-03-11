using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Api.PostDataObjects
{
	public class RegistrySearchPostData
	{
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }

		public int[] Categories { get; set; }
		//public int[] Su
	}
}