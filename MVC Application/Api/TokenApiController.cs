using Monei.Core.BusinessLogic;
using Monei.MvcApplication.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Monei.MvcApplication.Api
{
    [RoutePrefix("api/token")]
    public class TokenApiController : ApiController
    {
        private readonly IAccountManager accountManager;
        private readonly IAccountSecurity accountSecurity;


        public TokenApiController(IAccountManager accountManager, IAccountSecurity accountSecurity)          
        {
            this.accountManager = accountManager;
            this.accountSecurity = accountSecurity;
        }

        // GET api/token/new
        [HttpGet()]
        public Guid Get(string username, string password)
        {
            var account = accountManager.Read(username);

            if(account == null)
               BadRequest($@"Account not found for user ""{username}"".");

            if (password.ToLowerInvariant() != account.Password.ToLowerInvariant())
                BadRequest($@"Wrong password.");

            return accountSecurity.GetApiTokenForAccount(account.Id);
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