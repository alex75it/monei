using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
//using System.Linq.Expressions;

namespace Monei.DataAccessLayer.SqlServer
{
	public class RegistryRepository : AbstractRepository<int, RegistryRecord>, IRegistryRepository 
	{
		public IList<RegistryRecord> ListRecods(Filters.RegistryFilters filters)
		{
			IList<RegistryRecord> records ;

			using(ISession session = OpenSession())
			{
				IQueryable<RegistryRecord> query = session.Query<RegistryRecord>().Where(r =>
					(!filters.AccountId.HasValue || r.Account.Id == filters.AccountId)
					&& r.Date >= filters.StartDate && r.Date <= filters.EndDate					
					&& (string.IsNullOrWhiteSpace(filters.TextToSearch) || r.Note.Contains(filters.TextToSearch))
					//&& (filters.OperationTypes.Any(op => op == r.OperationType))
					&& (filters.OperationTypes.Contains(r.OperationType))
					&& (!filters.ShowOnlyTaxDeductible || r.IsTaxDeductible)
					&& (filters.IncludeSpecialEvent || !r.IsSpecialEvent)
					&& (filters.Amount == 0 || r.Amount == filters.Amount)
					);

				//IQueryOver<RegistryRecord> query = session.QueryOver<RegistryRecord>().Where(r =>
				//	(!filters.AccountId.HasValue || r.Account.Id == filters.AccountId)
				//	&& r.Date >= filters.StartDate && r.Date <= filters.EndDate

				//	&& (string.IsNullOrWhiteSpace(filters.TextToSearch) || r.Note.Contains(filters.TextToSearch))
				//	&& (!filters.ShowOnlyTaxDeductible || r.IsTaxDeductible)
				//	&& (filters.IncludeSpecialEvent || !r.IsSpecialEvent)
				//	);

				//Expression<Func<RegistryRecord, bool>> condition = new Expression<Func<RegistryRecord, bool>>();
				//Amon Tobin
				
				if(filters.SubcategoryIds.Count > 0)
				{
					//foreach(int id filters.SubcategoryIds)
						//query.Where(r => r.Subcategory.Id == id);
					query = query.Where( r => filters.SubcategoryIds.Contains(r.Subcategory.Id));
				}
				else
				{
					query = query.Where(r => !filters.CategoryId.HasValue || r.Category.Id == filters.CategoryId);
				}

				records = query.ToList();
			}

			return records;
		}

		//public IEnumerable<RegistryRecord> TestList()
		//{
		//	RegistryRecord aliasRecord = null;
		//	Category aliasCategory = null;
		//	using (ISession session = OpenSession())
		//	{
		//		IQueryOver<RegistryRecord, RegistryRecord> query = session.QueryOver<RegistryRecord>( () => aliasRecord)
		//			.JoinAlias<Category>( aliasRecord => aliasRecord.Category, () => aliasCategory, NHibernate.SqlCommand.JoinType.InnerJoin, Restrictions.On( () => aliasRecord.Category.Id ==  aliasCategory.Id ))
		//		//	.Where(m=> m.)
		//		//	.List();

		//		return session.QueryOver<RegistryRecord>()
		//			.JoinAlias(r => r.Category, () => aliasCategory)
		//			//.Where(m=> m.)
		//			.Select( x =>  x, x=> x.CreationAccount, x => aliasCategory.CreationDate)
		//			.List();
		//	}
		//}

		public RegistryRecord AddRecord(RegistryRecord record)
		{
			// todo: move logic to BL

			record.CreationDate = DateTime.Now;

			int recordId = Create(record);
			record = Read(recordId);
			
			//using(ISession session = OpenSession())
			//{
			//	recordId = session.Save(record) as RegistryRecord;
			//	session.Flush();
			//}
			return record;
		}

		public void UpdateRecord(RegistryRecord record)
		{			
			using (ISession session = OpenSession())
			{
				session.Update(record);
				session.Flush();
			}
		}


		public void DeleteRecord(int recordId)
		{
			Delete(recordId);
		}
		
		public bool IsSubcategoryUsed(int subcategoryId)
		{
			using (IStatelessSession session = OpenStatelessSession())
			{
				return session.Query<RegistryRecord>().Any(r => r.Subcategory.Id == subcategoryId);
			}
		}


	}
}
