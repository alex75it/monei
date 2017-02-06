using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.Core.DataAnalysis;
using Monei.Core.DataAnalysis.BusinessObjects;

namespace Monei.MvcApplication.Api
{
    //[Authorize]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiControllerBase
    {
        private Engine engine;

        public DashboardController(Engine engine)
        {
            this.engine = engine;
        }

        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("{year:int}")]
        public YearData Get(int year)
        {
            YearData data = engine.GetYearData(CurrentAccount.Id, year);
            return data;
        }
    }
}