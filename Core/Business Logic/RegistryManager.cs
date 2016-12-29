using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Core
{

    public interface IRegistryManager
    {
        int CreateRecord(RegistryRecord record);
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
            var newRecord = registryRepository.AddRecord(record);
            return newRecord.Id;
        }
    }
}
