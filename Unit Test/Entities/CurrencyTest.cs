using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Entities
{
	[TestFixture]
	public class CurrencyTest
	{
		[Test]
		public void Equals_Works()
		{
			Currency currency_1 = new Currency() { Id = 1 };
			Currency currency_2 = new Currency() { Id = 2 };
			Currency currency_1bis = new Currency() { Id = 1 };

			Assert.IsFalse(currency_1.Equals(null));
			currency_1.ShouldNotEqual(currency_2);
			currency_1.ShouldEqual(currency_1bis);
		}
	}
}
