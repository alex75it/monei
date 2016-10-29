using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Core.Enums
{
    public enum LoginResult
    {
        Ok = 0,
        UsernameNotFound = 10,
        WrongPassword = 20,
        InternalError = 30,
    }
}
