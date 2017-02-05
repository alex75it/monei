using Monei.DataAccessLayer.SqlServer;
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

        internal TestDataProvider()
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure(); // it fail ONLY in debug mode, just go on !
            sessionFactory = configuration.BuildSessionFactory();            
        }

        internal Category GetTestCategory()
        {
            Category category = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                category = session.QueryOver<Category>().Take(1).List().First();
            }

            return category;
        }

        internal Subcategory GetTestSubcategory(int categoryId)
        {
            Subcategory subcategory = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                subcategory = session.QueryOver<Subcategory>().Where(s => s.Category.Id == categoryId).Take(1).List().First();
            }

            return subcategory;
        }

        internal Currency GetTestCurrency()
        {
            using (ISession session = sessionFactory.OpenSession())
                return session.QueryOver<Currency>().Take(1).List().First();
        }

        internal Account GetTestAccount()
        {
            string testAccountUsername = "test";
            var currency = GetTestCurrency();

            using (ISession session = sessionFactory.OpenSession())
            {
                // using ToLowerInvariant() cause a strange exception...
                //Account account = session.QueryOver<Account>().Where(a => a.Username.ToLowerInvariant() == testAccountUsername).List().FirstOrDefault();
                Account account = session.QueryOver<Account>().Where(a => a.Username == testAccountUsername).List().FirstOrDefault();

                if (account == null)
                {
                    account = new Account() {
                        Username = testAccountUsername,
                        Password = "test",
                        Guid = Guid.NewGuid(),
                        Currency = currency,
                    };

                   session.Save(account);
                }

                return account;
            }
        }

        internal Guid GetValidApiToken()
        {
            Account account = GetTestAccount();

            using (ISession session = sessionFactory.OpenSession())
            {
                var apiToken = session.QueryOver<ApiToken>().Where(t => t.AccountId == account.Id).SingleOrDefault();
                if (apiToken == null)
                {
                    apiToken = ApiToken.Create(account.Id, TimeSpan.FromHours(2));
                    session.Save(apiToken);
                }
                else if (apiToken.IsExpired)
                {
                    apiToken.ExpiryDate = DateTime.UtcNow.AddHours(2);
                    session.Update(apiToken);                    
                }

                session.Flush();
                return apiToken.Id;
            }            
        }
    }
}
