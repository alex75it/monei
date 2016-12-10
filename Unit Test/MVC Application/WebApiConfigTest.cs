﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Monei.MvcApplication;
using NUnit.Framework;
using Monei.MvcApplication.Api;
using Should;

namespace Monei.Test.UnitTest.MvcApplication
{
    [TestFixture, Category("Web API")]
    public class WebApiConfigTest
    {
        [Test]
        public void Register()
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
