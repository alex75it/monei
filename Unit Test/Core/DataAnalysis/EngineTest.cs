using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Core.DataAnalysis;
using Monei.DataAccessLayer.Interfaces;
using NUnit.Framework;
using FakeItEasy;
using Should;

namespace Monei.Test.UnitTest.Core.DataAnalysis
{
	[TestFixture]
	public class EngineTest
	{
		[Test]
		public void GetYearData_should_()
		{
			int accountId = 1;
			int year = DateTime.Today.Year;
			IRegistryRepository registryRepository = A.Fake<IRegistryRepository>();
			//A.Dummy<IRegistryRepository>();

			Engine engine = new Engine(registryRepository);

			var data = engine.GetYearData(accountId, year);

			data.Income.ShouldEqual(0);
			data.Outcome.ShouldEqual(0);
			data.Result.ShouldEqual(0);
		}
	}
}
