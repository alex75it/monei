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
    public class ApiTokenRepository : RepositoryBase<Guid, ApiToken>, IApiTokenRepository
    {
        public ApiTokenRepository(ISessionFactoryProvider sessionFactoryProvider) : base(sessionFactoryProvider)
        {
        }

        public int GetAccountId(Guid token)
        {
            throw new NotImplementedException();
        }

        public ApiToken GetAccountToken(int accountId)
        {
            throw new NotImplementedException();
        }

        void IApiTokenRepository.Create(ApiToken token)
        {
            throw new NotImplementedException();
        }
    }
}
