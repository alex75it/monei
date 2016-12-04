using Monei.DataAccessLayer.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;

namespace Monei.Test.UnitTest.DataAccessLayer.Exceptions
{
    [TestFixture, Category("Data Access Layer")]
    internal class TooLongCategoryNameExceptionTest
    {
        [Test]
        public void MaxLength_should_BeTheRightValue()
        {
            int maxLength = 25;
            var exception = new TooLongCategoryNameException(maxLength);
            exception.MaxLength.ShouldEqual(maxLength);
        }
    }
}
