using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using NHibernate;

namespace Monei.DataAccessLayer.SqlServer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">Entity managed by this repository</typeparam>
    public abstract class RepositoryBase<TKey, TEntity>: IRepository<TKey, TEntity> //where TEntity: BaseEntity
    {
        private string connectionString;

        private ISessionFactory sessionFactory;

        public RepositoryBase(ISessionFactoryProvider sessionFactoryProvider)
        {
            sessionFactory = sessionFactoryProvider.GetSessionFactory();
        }
         
        protected string ConnectionString { get
            {
                if (connectionString == null)
                {
                    try
                    {
                        connectionString = ConfigurationManager.ConnectionStrings["monei"].ConnectionString;
                    }
                    catch (Exception exc)
                    {
                        throw new Exception("Fail to load \"monei\" connection string from .config file.", exc);
                    }
                }
                return ConnectionString;
            }
        }                

        protected ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }

        protected IStatelessSession OpenStatelessSession()
        {
            return sessionFactory.OpenStatelessSession();
        }

        public TKey Create(TEntity data)
        {
            using (ISession session = OpenSession())
            {
                return (TKey)session.Save(data);
            }
        }

        public TEntity Read(TKey key)
        {
            using (ISession session = OpenSession())
            {
                return session.Get<TEntity>(key);
            }
        }

        public void Update(TEntity data)
        {
            using (ISession session = OpenSession())
            {
                session.Update(data);
                session.Flush();
            }
        }

        public void Delete(TKey key)
        {
            using (ISession session = OpenSession())
            {
                TEntity data = Read(key);
                if (data != null)
                    session.Delete(data);
                session.Flush();
            }
        }

    }
}
