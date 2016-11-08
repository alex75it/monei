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
        private ISessionFactory sessionFactory;
        public ISessionFactory GetSessionFactory()
        {
            if (sessionFactory == null)
            {
                var configuration = new NHibernate.Cfg.Configuration();
                configuration.Configure(); // require a NHibernate working section in .config file

                sessionFactory = configuration.BuildSessionFactory();
            }

            return sessionFactory;
        }
    }
}
