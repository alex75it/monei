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

namespace Monei.DataAccessLayer.SqlServer
{
    public class RegistryRepository : RepositoryBase<int, RegistryRecord>, IRegistryRepository 
    {
        public RegistryRepository(ISessionFactoryProvider sessionFactoryProvider) : base(sessionFactoryProvider)
        {
        }

        public IList<RegistryRecord> ListRecords(Filters.RegistryFilters filters)
        {
            filters.Normalize();

            IList<RegistryRecord> records ;

            using(ISession session = OpenSession())
            {
                IQueryable<RegistryRecord> query = session.Query<RegistryRecord>().Where(r =>
                    (!filters.AccountId.HasValue || r.Account.Id == filters.AccountId)
                    && r.Date >= filters.StartDate && r.Date <= filters.EndDate					
                    && (string.IsNullOrWhiteSpace(filters.TextToSearch) || r.Note.Contains(filters.TextToSearch))
                    && (filters.OperationTypes.Contains(r.OperationType))
                    && (!filters.ShowOnlyTaxDeductible || r.IsTaxDeductible)
                    && (filters.IncludeSpecialEvent || !r.IsSpecialEvent)
                    && (filters.Amount == 0 || r.Amount == filters.Amount)
                    );
                
                if(filters.SubcategoryIds.Count > 0)
                {
                    query = query.Where( r => filters.SubcategoryIds.Contains(r.Subcategory.Id));
                }
                else if(filters.Categories != null && filters.Categories.Count() > 0)
                {
                    query = query.Where(r => filters.Categories.Contains(r.Category.Id));
                }

                records = query.ToList();
            }

            return records;
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
