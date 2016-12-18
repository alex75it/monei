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
    [TestFixture, Category("Repository")]
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
            Guid tokenId = Guid.NewGuid();
            int accountId = Helper.GetTestAccount().Id;

            ApiToken token = new ApiToken() {
                Id = tokenId,
                AccountId = accountId,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(5)
            };

            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                session.Save(token);
            };

            try
            {
                var loadedAccountId = apiTokenRepository.GetAccountId(tokenId);

                loadedAccountId.ShouldEqual(accountId);
            }
            finally
            {
                DeleteToken(tokenId);
            }            
        }

        [Test]
        public void GetAccountToken()
        {
            var accountId = Helper.GetTestAccount().Id;

            var token = apiTokenRepository.GetAccountToken(accountId);

            token.ShouldNotBeNull();
        }

        [Test]
        public void GetAccountToken_when_TokenNotExists()
        {
            var accountId = Helper.GetTestAccount().Id;

            Action action = () => apiTokenRepository.GetAccountToken(accountId);

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Create()
        {
            Guid tokenId = Guid.NewGuid();
            var accountId = Helper.GetTestAccount().Id;

            ApiToken token = new ApiToken() {
                Id = tokenId,
                AccountId = accountId,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(5)
            };

            // execute
            Guid returnedTokenId = apiTokenRepository.Create(token);

            returnedTokenId.ShouldEqual(tokenId);

            ApiToken tokenForCheck = ReadToken(tokenId);

            tokenForCheck.ShouldNotBeNull();
            tokenForCheck.Id.ShouldEqual(tokenId);
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
    }
}
