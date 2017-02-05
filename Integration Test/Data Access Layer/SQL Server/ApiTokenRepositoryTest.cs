using Monei.DataAccessLayer.SqlServer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;
using Monei.DataAccessLayer.Exceptions;
using Monei.Entities;
using Monei.DataAccessLayer.Interfaces;
using NHibernate.Linq;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
    [TestFixture, Category("Repository"), Category("API Token")]
    public class ApiTokenRepositoryTest : RepositoryTestBase
    {
        private ApiTokenRepository apiTokenRepository;
        private ISessionFactoryProvider sessionFactoryProvider;

        [SetUp]
        public void SetUp()
        {
            apiTokenRepository = new ApiTokenRepository(new SessionFactoryProvider());
            sessionFactoryProvider = new SessionFactoryProvider();
        }

        [Test]
        public void GetAccountId()
        {
            int accountId = Helper.GetTestAccount().Id;

            DeleteTokenOfAccount(accountId);

            ApiToken token = ApiToken.Create(accountId, TimeSpan.FromMinutes(5));

            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                session.Save(token);
                session.Flush();
            };

            try
            {
                var loadedAccountId = apiTokenRepository.GetAccountId(token.Id);

                loadedAccountId.ShouldEqual(accountId);
            }
            finally
            {
                DeleteToken(token.Id);
            }            
        }

        [Test]
        public void GetAccountId_when_TokenNotExists_should_ThrowsArgumentException()
        {
            Guid invalidTokenId = Guid.NewGuid();

            Action action = () => apiTokenRepository.GetAccountId(invalidTokenId);

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void GetAccountToken()
        {
            var accountId = Helper.GetTestAccount().Id;

            CreateToken(accountId, DateTime.UtcNow.AddMinutes(5));

            try
            {
                var token = apiTokenRepository.GetAccountToken(accountId);

                token.ShouldNotBeNull();
                token.AccountId.ShouldEqual(accountId);
            }
            finally
            {
                DeleteTokenOfAccount(accountId);
            }
        }

        [Test]
        public void GetAccountToken_when_TokenNotExists_returnNull()
        {
            var accountId = Helper.GetTestAccount().Id;
            DeleteTokenOfAccount(accountId);

            var token = apiTokenRepository.GetAccountToken(accountId);

            token.ShouldBeNull();
        }

        [Test]
        public void Create()
        {
            var accountId = Helper.GetTestAccount().Id;
            ApiToken token = ApiToken.Create(accountId, TimeSpan.FromMinutes(5));
            
            // execute
            Guid returnedTokenId = apiTokenRepository.Create(token);

            try
            {                
                returnedTokenId.ShouldEqual(token.Id);
                ApiToken tokenForCheck = ReadToken(token.Id);
                tokenForCheck.ShouldNotBeNull();
                tokenForCheck.Id.ShouldEqual(token.Id);
            }
            finally
            {
                DeleteTokenOfAccount(accountId);
            }
        }

        private ApiToken ReadToken(Guid tokenId)
        {
            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
                return session.Query<ApiToken>().Where(t => t.Id == tokenId).Single();
        }

        private void DeleteToken(Guid tokenId)
        {
            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                var token = session.Query<ApiToken>().Where(t => t.Id == tokenId).SingleOrDefault();
                if (token != null)
                {
                    session.Delete(token);
                    session.Flush();
                }
            };
        }

        private void DeleteTokenOfAccount(int accountId)
        {
            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                var token = session.Query<ApiToken>().Where(t => t.AccountId == accountId).SingleOrDefault();
                if (token != null)
                {
                    session.Delete(token);
                    session.Flush();
                }
            };
        }
        private void CreateToken(int accountId, DateTime expiryDate)
        {
            ApiToken token = ApiToken.Create(accountId, TimeSpan.FromMinutes(5));

            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                session.Save(token);
                session.Flush();
            }
        }
    }
}
