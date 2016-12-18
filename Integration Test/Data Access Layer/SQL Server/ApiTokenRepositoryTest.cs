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
            Guid tokenId = Guid.NewGuid();
            int accountId = Helper.GetTestAccount().Id;

            DeleteTokenOfAccount(accountId);

            ApiToken token = new ApiToken() {
                Id = tokenId,
                AccountId = accountId,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMinutes(5)
            };

            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                session.Save(token);
                session.Flush();
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
        public void GetAccountToken_when_TokenNotExists()
        {
            var accountId = Helper.GetTestAccount().Id;

            DeleteTokenOfAccount(accountId);

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

            try
            {
                returnedTokenId.ShouldEqual(tokenId);

                ApiToken tokenForCheck = ReadToken(tokenId);

                tokenForCheck.ShouldNotBeNull();
                tokenForCheck.Id.ShouldEqual(tokenId);
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
            var token = new ApiToken()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = expiryDate,
            };

            using (var session = sessionFactoryProvider.GetSessionFactory().OpenSession())
            {
                session.Save(token);
                session.Flush();
            }
        }
    }
}
