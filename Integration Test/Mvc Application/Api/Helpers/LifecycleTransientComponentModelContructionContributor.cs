using Castle.MicroKernel.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.MicroKernel;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
    internal class LifestyleSingletonComponentModelContruction : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            model.LifestyleType = LifestyleType.Singleton;
        }
    }
}
