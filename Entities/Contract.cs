using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Entities
{
	public class Contract :EntityBase<int>
	{
		public enum ContractRecurrence { 
			Monthly,
			BiMonthly,
			HalfYearly,
			Yearly,
			BiYearly,
		}

		public Account Account { get; set; }
		public string Supplier { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Note { get; set; }
		public decimal DueAmount { get; set; }
		public ContractRecurrence Recurrence { get; set; }

	}

}
