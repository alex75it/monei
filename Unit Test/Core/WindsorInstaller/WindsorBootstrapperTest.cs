using Monei.MvcApplication.Controllers;
using Monei.MvcApplication.Core.WindsorInstallers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Test.UnitTest.Core.WindsorInstaller
{
    [TestFixture]
    public class WindsorBootstrapperTest
    {


        public void Initialize_should_MakeBusinessLogicResolvable()
        {
            using (WindsorBootstrapper bootstrapper = new WindsorBootstrapper())
            {
                bootstrapper.Initialize();


                //AccountController controller = new WindsorContainer
            }




        }


        //public void Initialize_should_MakeControllersResolvable()
        //{
        //    WindsorBootstrapper bootstrapper = new WindsorBootstrapper();

        //    bootstrapper.Initialize();

        //    AccountController controller = new WindsorContainer()


        //}
    }
}