using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities.Interfaces;

namespace Monei.Entities
{
    public enum OperationType { Income = +1, Outcome = -1, Transfer = 0, }

    
    public class RegistryRecord : EntityBase<int>, ICsvFormattable
    {
        public const Decimal MAX_AMOUNT = 99999;
        public const Decimal MIN_AMOUNT = -99999;
        public const int NOTE_MAX_LENGTH = 500;

        public const string FIELD_ACCOUNT = "Account";
        public const string FIELD_DATE = "Date";
        public const string FIELD_CATEGORY = "Category";
        public const string FIELD_SUBCATEGORY = "Subcategory";

        
        public virtual Account Account { get; set; }
        [Required]
        public virtual DateTime Date { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode=true)]
        public virtual decimal Amount { get; set; }
        public OperationType OperationType {get; set;}    
        public virtual string Note { get; set; }
        public virtual bool IsTaxDeductible { get; set; }
        public virtual bool IsSpecialEvent { get; set; }
        public virtual Category Category { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        public decimal RealAmount { get {
            return OperationType == Entities.OperationType.Transfer ? 0 :
                OperationType == Entities.OperationType.Income ? Amount :
                -Amount;
        } }
        
        string ICsvFormattable.CreateHeadersRow()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};\"{6}\";\"{7}\";\"{8}\"",
                "Year",
                "Month",
                "Date",
                "Income",
                "Outcome",
                "Transfer",
                "Category",
                "Subcategory",
                "Note"
            );
        }

        string ICsvFormattable.CreateRow()
        {
            return string.Format(@"{0};{1};{2};{3};{4};{5};""{6}"";""{7}"";""{8}"";",
                Date.Year,
                Date.Month,
                Date.ToShortDateString(),  // 0
                OperationType == OperationType.Income ? Amount.ToString("n2") : "",
                OperationType == OperationType.Outcome ? Amount.ToString("n2") : "",
                OperationType == OperationType.Transfer ? Amount.ToString("n2") : "",
                Category.Name,	// 3
                Subcategory != null ? Subcategory.Name: "",		// 4
                Note	// 5
            );
        }

        
    }//class
}
