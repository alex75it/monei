using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Interfaces
{
    public interface IApiTokenRepository
    {
        int GetAccountId(Guid token);
        ApiToken GetAccountToken(int accountId);

        void Delete(Guid tokenId);

        void Create(ApiToken token);
    }
}
