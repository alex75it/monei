using Monei.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
    public class TestDataProvider
    {
        private ISessionFactory sessionFactory;

        public TestDataProvider()
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure(); // it fail ONLY in debug mode, just go on !
            sessionFactory = configuration.BuildSessionFactory();            
        }

        public Category GetTestCategory()
        {
            Category category = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                category = session.QueryOver<Category>().Take(1).List().First();
            }

            return category;
        }
        public Subcategory GetTestSubcategory(int categoryId)
        {
            Subcategory subcategory = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                subcategory = session.QueryOver<Subcategory>().Where(s => s.Category.Id == categoryId).Take(1).List().First();
            }

            return subcategory;
        }
    }
}
