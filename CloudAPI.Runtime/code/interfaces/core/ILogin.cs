using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{

    public interface ILogin
    {
        LoginType LoginType {get; set;} 
        string Username {get; set;}
        string Profile {get; set;}
        string AuthToken {get; set;}

    }
}
