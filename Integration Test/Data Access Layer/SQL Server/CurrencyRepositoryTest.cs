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
	[TestFixture]
	public class CurrencyRepositoryTest : RepositoryTestBase
	{

		[Test]
		public void Delete()
		{
			string code = "YYY";
			string name = "YYY";
			string symbol = "è";

			Currency currency = new Currency()
			{
				Code = code,
				Name = name,
				Symbol = symbol,
			};


			try
			{
				//currency = CurrencyRepository.Create(currency);

				throw new Exception("Create method does not exists");
			}
			catch (Exception exc)
			{
				Assert.Inconclusive("Fail to create Currency: " + exc.ToString());
				return;
			}

			try
			{
				CurrencyRepository.Delete(currency.Id);
			}
			catch (Exception exc)
			{
				Assert.Fail(exc.ToString());
			}

			Currency deletedCurrency = CurrencyRepository.Read(currency.Code);

			Assert.IsNull(deletedCurrency, "Re-loaded Currency is not null");

		}

	}
}
