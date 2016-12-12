using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;

namespace Monei.DataAccessLayer.SqlServer
{
    public class ApiTokenRepository : RepositoryBase<int, ApiToken>, IApiTokenRepository
    {
        public ApiTokenRepository(ISessionFactoryProvider sessionFactoryProvider) : base(sessionFactoryProvider)
        {
        }

        public Guid CreateNewToken(int accountId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid key)
        {
            throw new NotImplementedException();
        }

        public ApiToken GetAccountToken(int accountId)
        {
            throw new NotImplementedException();
        }

        public ApiToken Read(Guid key)
        {
            throw new NotImplementedException();
        }

        Guid IRepository<Guid, ApiToken>.Create(ApiToken entity)
        {
            throw new NotImplementedException();
        }
    }
}
