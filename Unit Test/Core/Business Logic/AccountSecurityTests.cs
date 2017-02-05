using Monei.Core.BusinessLogic;
using Monei.DataAccessLayer.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Should;
using Monei.Entities;

namespace Monei.Test.UnitTest.Core.BusinessLogic
{
    [TestFixture]
    public class AccountSecurityTests
    {
        AccountSecurity accountSecurity;
        private IAccountRepository accountRepository;
        private IApiTokenRepository apiTokenRepository;                  

        [SetUp]
        public void SetUp()
        {
            accountRepository = A.Fake<IAccountRepository>();
            apiTokenRepository = A.Fake<IApiTokenRepository>();
            accountSecurity = new AccountSecurity(accountRepository, apiTokenRepository);
        }

        [Test]
        public void GetApiTokenForAccount_when_TokenAlreadyExists_should_ReturnTheToken()
        {
            int accountId = 1;

            ApiToken previousToken = ApiToken.Create(accountId, TimeSpan.FromMinutes(5));

            A.CallTo(() => apiTokenRepository.GetAccountToken(A<int>.Ignored))
                .Returns(previousToken);
            accountSecurity = new AccountSecurity(accountRepository, apiTokenRepository);

            // execute
            Guid token = accountSecurity.GetApiTokenForAccount(accountId);

            token.ShouldEqual(previousToken.Id);
        }

        [Test]
        public void GetApiTokenForAccount_when_TokenNotExists_should_CreateNewToken()
        {
            int accountId = 1;

            A.CallTo( () => apiTokenRepository.GetAccountToken(A<int>.Ignored))
                .Returns(null);
            accountSecurity = new AccountSecurity(accountRepository, apiTokenRepository);

            // execute
            Guid token = accountSecurity.GetApiTokenForAccount(accountId);

            token.ShouldNotEqual(Guid.Empty);
            A.CallTo(() => apiTokenRepository.Create(A<ApiToken>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void GetApiTokenForAccount_when_TokenIsExpired_should_CreateANewToken()
        {
            int accountId = 1;

            ApiToken expiredToken = ApiToken.Create(accountId, TimeSpan.FromMinutes(5));
            expiredToken.ExpiryDate = DateTime.UtcNow.AddMinutes(-5);  // expired

            A.CallTo(() => apiTokenRepository.GetAccountToken(A<int>.Ignored))
                .Returns(expiredToken);
            accountSecurity = new AccountSecurity(accountRepository, apiTokenRepository);

            // execute
            Guid token = accountSecurity.GetApiTokenForAccount(accountId);

            token.ShouldNotEqual(Guid.Empty);            
            A.CallTo(() => apiTokenRepository.Create(A<ApiToken>.Ignored)).MustHaveHappened();            
        }
    }
}
