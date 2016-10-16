using Monei.DataAccessLayer.SqlServer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Test.IntegrationTest.Data_Access_Layer.SQL_Server
{
    [TestFixture]
    public class SessionFactoryProviderTest
    {
        [Test]
        public void GetSessionFactory_when_CalledMultipleTimes_should_ReturnAValidSession()
        {
            SessionFactoryProvider provider = new SessionFactoryProvider();

            var sessionFactory = provider.GetSessionFactory();
            using (sessionFactory.OpenSession())
            {
                // ...
            }

            using (sessionFactory.OpenSession())
            {
                // ...
            }
        }
    }
}
