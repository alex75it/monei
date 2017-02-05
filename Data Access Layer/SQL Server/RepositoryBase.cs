﻿using System;
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
        private static ISessionFactory sessionFactory;
        private ISessionFactoryProvider sessionFactoryProvider;


        public RepositoryBase(ISessionFactoryProvider sessionFactoryProvider)
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
        }  
        protected ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        protected IStatelessSession OpenStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        public TKey Create(TEntity data)
        {
            using (ISession session = OpenSession())
            {
                var key = (TKey)session.Save(data);
                session.Flush();
                return key;
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

        /*protected internal*/ // InternalsVisibleTo attribute is not working
        public
            ISessionFactory SessionFactory {
            get {
                if(sessionFactory == null)
                    sessionFactory = sessionFactoryProvider.GetSessionFactory();
                return sessionFactory;
            }
        }
    }
}
