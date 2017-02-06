using Monei.DataAccessLayer.Filters;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Core.BusinessLogic
{

    public interface IRegistryManager
    {
        int CreateRecord(RegistryRecord record);

        IEnumerable<RegistryRecord> ListRecords(RegistryFilters filters);
    }

    public class RegistryManager : IRegistryManager
    {
        private IRegistryRepository registryRepository;

        public RegistryManager(IRegistryRepository registryRepository)
        {
            this.registryRepository = registryRepository;
        }

        public int CreateRecord(RegistryRecord record)
        {
            var newId = registryRepository.Create(record);
            return newId;
        }

        public IEnumerable<RegistryRecord> ListRecords(RegistryFilters filters)
        {
            return registryRepository.ListRecords(filters);
        }
    }
}
