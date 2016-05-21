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
    public class RepositoryBase<TKey, TEntity>: IRepository<TKey, TEntity> //where TEntity: BaseEntity
    {
        private string connectionString;
         
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
                        throw new Exception("Fail to load connection string", exc);
                    }
                }
                return ConnectionString;
            }
         }
                
        //Nhibernate session factory
        //Component.For<ISessionFactory>().UsingFactoryMethod(CreateNhSessionFactory).LifeStyle.Singleton,

        protected ISessionFactory GetSessionFactory()
        { 
            var configuration = new NHibernate.Cfg.Configuration();
            //configuration.AddAssembly("Monei.Entities");
            //configuration.Configure(); // it fail ONLY in debug mode !
            //Console.WriteLine("Configuration is OK");

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            //NHibernate.Dialect.MsSql2012Dialect
    
            return sessionFactory;
        }

        protected ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
            //return GetSessionFactory().GetCurrentSession;
        }

        protected IStatelessSession OpenStatelessSession()
        {
            return GetSessionFactory().OpenStatelessSession();
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
