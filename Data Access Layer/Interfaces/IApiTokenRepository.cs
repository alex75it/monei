using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.Interfaces
{
    public interface IApiTokenRepository : IRepository<Guid, ApiToken>
    {
        ApiToken GetAccountToken(int accountId);
        Guid CreateNewToken(int accountId);
    }
}
