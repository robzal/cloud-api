using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{

    public class Login : BaseObject, ILogin
    {
        public LoginType LoginType {get; set;} 
        public string Username {get; set;}
        public string Profile {get; set;}
        public string AuthToken {get; set;}

        public Login() : base()
        {

        }
        public Login (string Username, string Password, LoginType loginType) : this() {
            //try auth
        } 
        public Login (string Token, LoginType loginType): this() {
            //thy auth
        }
    }
}
