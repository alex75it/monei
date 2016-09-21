using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Test.UnitTest
{
	public class TestBase
	{
		protected DateTime minSqlDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
		protected DateTime maxSqlDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

		protected bool IsValidSqlDate(DateTime date)
		{
			return date >= minSqlDate
			       && date <= maxSqlDate;
		}


        protected void AssertExceptionIsRaised(Action action, Exception expectedException)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                if (exc.GetType() == expectedException.GetType())
                    if (exc.Message == expectedException.Message)
                        Assert.Pass();
                    else
                        Assert.Fail("Exception message is not as expected one.\nExpected message: " + expectedException.Message + "\nActual message: " + exc.Message);
                else
                    Assert.Fail("Raised Exception type is " + exc.GetType().Name + " insteasd of " + expectedException.GetType().Name);
            }

            Assert.Fail("It was expected an Exception were thrown");
        }

        protected void AssertExceptionIsRaised(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail("An Exception was expected but it was not raised.");
        }
    }
}
