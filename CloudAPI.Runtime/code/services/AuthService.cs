using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService() 
        {
            this.ServiceType = ServiceType.AUTHENTICATION;
        }
    }
}