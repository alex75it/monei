using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.Core.BusinessLogic;
using log4net;
using Monei.MvcApplication.Api.PostDataObjects;

namespace Monei.MvcApplication.Api
{
    [RoutePrefix("api/token")]
    public class TokenApiController : ApiController
    {
        public IAccountManager AccountManager { get; set; }
        public IAccountSecurity AccountSecurity { get; set; }
        private ILog logger;

        public TokenApiController()          
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        [HttpGet(), Route("ping")]
        public string Ping()
        {
            if (AccountManager == null) throw new Exception("AccountManager not injected");
            if (AccountSecurity == null) throw new Exception("AccountSecurity not injected");
            return "pong";
        }

        // POST api/token/new
        [HttpPost(), Route("new")]
        //public Guid New([FromBody]string username, [FromBody]string password)
        public Guid New(NewApiTokenPostData data)
        {
            var account = AccountManager.Read(data.Username);

            if(account == null)
               BadRequest($@"Account not found for user ""{data.Username}"".");

            if (data.Password.ToLowerInvariant() != account.Password.ToLowerInvariant())
                BadRequest($@"Wrong password.");

            return AccountSecurity.GetApiTokenForAccount(account.Id);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}