using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Monei.Test.IntegrationTest.Mvc_Application.Api
{
	public class ApiControllerTestBase
	{


		//protected void InitializeController(ApiController controller)
		//{ 
		
		//}

		protected HttpConfiguration GetConfiguration()
		{
			HttpConfiguration configuration = new HttpConfiguration();		

			return configuration;
		}

	}
}
