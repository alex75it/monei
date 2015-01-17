using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Core.BusinessObjects
{
	public class JsonRegistryRecord
	{


		public string Account { get; set; }

		public virtual DateTime Date { get; set; }
		public virtual decimal Amount { get; set; }
		//public OperationType OperationType { get; set; }
		public virtual string Note { get; set; }
		public virtual bool IsTaxDeductible { get; set; }
		public virtual bool IsSpecialEvent { get; set; }
		//public virtual Category Category { get; set; }
		//public virtual Subcategory Subcategory { get; set; }
	}
}