using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
    [TestFixture, Category("Data Access Layer")]
    public class CurrencyRepositoryTest : RepositoryTestBase
    {

        [Test]
        [TestCase(Currency.EUR_CODE)]
        [TestCase(Currency.USD_CODE)]
        public void Read(string code)
        {
            var currency = CurrencyRepository.Read(code);
            currency.ShouldNotBeNull();
            currency.Code.ShouldEqual(code);
        }

        [Test]
        public void Reads_when_CurrencyDoesNotExists_should_RaiseSpecificException()
        {
            string currencyCode = "YYY";
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrencyRepository.Read(currencyCode));
        }

    }
}
