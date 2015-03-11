using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Core.DataAnalysis.BusinessObjects;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Core.DataAnalysis.BusinessObjects
{
	[TestFixture]
	public class MonthDataTest
	{
		[Test]
		public void Ctor()
		{
			var data = new MonthData(1);
			data.Result.ShouldEqual(0m);
		}
	}
}
