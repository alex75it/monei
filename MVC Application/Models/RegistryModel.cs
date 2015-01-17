using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using MongoDB.Bson;

namespace Monei.MvcApplication.Models
{
    public class Resume
    {
        public decimal Transfer { get; set; }
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
        public decimal Balance { get; set; }

		public decimal IncomePercent { get { return HasIncomeOutcomeValues ? (Income / (Outcome + Income)) * 100: 0; } }
		public decimal OutcomePercent { get { return HasIncomeOutcomeValues ? (Outcome / (Outcome + Income)) * 100: 0; } }
		public bool HasIncomeOutcomeValues { get { return (Outcome != 0 || Income != 0); } }
    }

	public class RegistryListModel
	{
		public SelectList CategorySelectList { get; set; }
		public SelectList AccountSelectList { get; set; }
		public RegistryFilters Filters { get; set; }
        private IEnumerable<RegistryRecord> records;
        public IEnumerable<RegistryRecord> Records { get { return records; } set { records = value; CalculateResume(); } }
        public Resume Resume { get; set; }

        private void CalculateResume()
        {
            Resume = new Resume()
            {
                Outcome = Records.Where(r => r.OperationType == OperationType.Outcome).Sum(r => r.Amount),
                Income = Records.Where(r => r.OperationType == OperationType.Income).Sum(r => r.Amount),
                Transfer = Records.Where( r => r.OperationType == OperationType.Transfer).Sum(r => r.Amount),
            };

            Resume.Balance = Resume.Income - Resume.Outcome;
        }		
	}

	
	public class RegistryCreateRecordModel { 
		public RegistryRecord Record {get; set;}
		//public IEnumerable<Category> Categories { get; set; }
		//public SelectList CategorySelectList { get; set; }
		//public SelectList Subcategory
		public ObjectId SelectedCategoryId { get; set; }
	}


	public class NewRegistryRecordModel {

		public RegistryRecord Record { get; set; }

	}

}