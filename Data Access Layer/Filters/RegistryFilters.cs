using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.DataAccessLayer.Filters
{

	public class RegistryFilters
	{
		public static string PERIOD_CURRENT_MONTH = "current month";

		public int? AccountId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int? CategoryId { get; set; }
		public IList<int> SubcategoryIds { get; set; }
		public string SelectedPeriod { get; set; }
		public string TextToSearch { get; set; }
		public decimal Amount { get; set; }
		public bool ShowOnlyTaxDeductible { get; set; }
		public bool IncludeSpecialEvent { get; set; }
		public OperationType[] OperationTypes { get; set; }

		public RegistryFilters()
		{
			StartDate = new DateTime(2000, 1, 1);
			EndDate = new DateTime(3000, 12, 31);		
			SubcategoryIds = new List<int>(0);
			OperationTypes = new OperationType[] { OperationType.Income, OperationType.Outcome };
		}
				

		public void Normalize()
		{
			if (EndDate == DateTime.MinValue)
				EndDate = DateTime.MaxValue;

			if (EndDate < StartDate)
			{
				DateTime temp = StartDate;
				StartDate = EndDate;
				EndDate = temp;
			}
		}

		public void SetOperationType(OperationType operation, bool enabled)
		{
			if (enabled && !OperationTypes.Contains(operation))
			{
				List<OperationType> list = OperationTypes.ToList();
				list.Add(operation);
				OperationTypes = list.ToArray();
			}
			else if(!enabled && OperationTypes.Contains(operation))
			{
				List<OperationType> list = OperationTypes.ToList();
				list.RemoveAll(o => o == operation);
				OperationTypes = list.ToArray();
			}
		}

	}//class

}
