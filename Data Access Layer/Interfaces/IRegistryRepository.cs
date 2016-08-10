using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;

namespace Monei.DataAccessLayer.Interfaces
{
	public interface IRegistryRepository
	{
		RegistryRecord Read(int id);
		IList<RegistryRecord> ListRecords(RegistryFilters filters);
		RegistryRecord AddRecord(RegistryRecord record);
		void UpdateRecord(RegistryRecord record);
		void DeleteRecord(int recordId);
		bool IsSubcategoryUsed(int subcategoryId);
	}
}
