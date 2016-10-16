using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.DataAccessLayer.SqlServer
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
    }
}
