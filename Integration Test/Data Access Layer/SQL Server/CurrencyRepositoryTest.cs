using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Exceptions;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Should;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{
	[TestClass]
	public class CurrencyRepositoryTest : RepositoryTestBase
	{

		
		[TestMethod]
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
