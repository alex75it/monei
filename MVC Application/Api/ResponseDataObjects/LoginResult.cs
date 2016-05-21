using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Api.ResponseDataObjects
{
    public enum LoginResult
    {
        Ok = 0,
        UsernameNotFound = 10,
        WrongPassword = 20,
        InternalError = 30,
    }
}