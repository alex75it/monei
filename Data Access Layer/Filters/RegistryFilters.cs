using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;

namespace Monei.DataAccessLayer.Filters
{

    public class RegistryFilters :BaseFilters
    {
        public const string PERIOD_CURRENT_MONTH = "current month";

        public int? AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[] Categories { get; set; }
        public IList<int> SubcategoryIds { get; set; }
        public string TextToSearch { get; set; }
        public decimal Amount { get; set; }
        public bool ShowOnlyTaxDeductible { get; set; }
        public bool IncludeSpecialEvent { get; set; }
        public OperationType[] OperationTypes { get; set; }

        public RegistryFilters()
        {
            StartDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            EndDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;		
            SubcategoryIds = new List<int>(0);
            OperationTypes = new OperationType[] { OperationType.Income, OperationType.Outcome, OperationType.Transfer };
        }
                

        public void Normalize()
        {
            if (EndDate == DateTime.MinValue)
                EndDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

            if (EndDate < StartDate)
            {
                DateTime temp = StartDate;
                StartDate = EndDate;
                EndDate = temp;
            }

            StartDate = base.NormalizeDate(StartDate);
            EndDate = base.NormalizeDate(EndDate);
        }
    }
}
