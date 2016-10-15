using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Monei.DataAccessLayer.SqlServer
{
    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        public ISessionFactory GetSessionFactory()
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure(); // it fail ONLY in debug mode, just go on !

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory;
        }
    }
}
