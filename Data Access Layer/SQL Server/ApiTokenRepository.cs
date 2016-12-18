using Monei.DataAccessLayer.SqlServer;
using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using NHibernate.Linq;

namespace Monei.DataAccessLayer.SqlServer
{
    public class ApiTokenRepository : RepositoryBase<Guid, ApiToken>, IApiTokenRepository
    {
        public ApiTokenRepository(ISessionFactoryProvider sessionFactoryProvider) : base(sessionFactoryProvider)
        {
        }

        public int GetAccountId(Guid tokenId)
        {
            var token = Read(tokenId);
            if (token == null)
                throw new ArgumentException("Token not found for the given Id");
            return token.AccountId;
        }

        public ApiToken GetAccountToken(int accountId)
        {
            using (var session = OpenSession())
            {
                var token = session.Query<ApiToken>().Where(t => t.AccountId == accountId).SingleOrDefault();
                if (token == null)
                    throw new ArgumentException("Token not found for the given Account Id");
                return token;
            }
        }

        void IApiTokenRepository.Create(ApiToken token)
        {
            throw new NotImplementedException();
        }
    }
}
