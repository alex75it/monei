using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.Entities;

namespace Monei.Tests.DataAccessLayer.Repository
{
	[TestClass]
	public class CurrencyRepositoryTest : RepositoryBaseTest
	{


		[TestMethod]
		public void Create()
		{
			string code = "XXX";
			string name = "XXX";
			string symbol = "X";

			Currency currency = new Currency() { 
				Code = code,
				Name = name,
				Symbol = symbol,
			};

			currency = CurrencyRepository.Create(currency);

			Assert.IsFalse(currency.Id == Currency.EMPTY_ID);
			Assert.AreEqual(currency.Name, name);
			Assert.AreEqual(currency.Code, code);
			Assert.AreEqual(currency.Symbol, symbol);

			
			try
			{
				CurrencyRepository.Delete(currency.Id);
			}
			catch { }

		}

		[TestMethod]
		public void TestDelete()
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
				currency = CurrencyRepository.Create(currency);
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
			catch (Exception exc){
				Assert.Fail(exc.ToString());
			}

			Currency deletedCurrency = CurrencyRepository.Read(currency.Code);

			Assert.IsNull(deletedCurrency, "Re-loaded Currency is not null");

		}

	}
}
