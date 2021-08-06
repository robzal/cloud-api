using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CloudAPI.Runtime
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {

        public AuthController(IHttpContextAccessor HttpContextAccessor) : base(HttpContextAccessor)
        {
        }
        
        [HttpGet]
        [Route("login")]
         public async Task<IActionResult> login(string username, string password)
        {
            var result = new Login();
            return Ok(result);
        }
        [HttpGet]
        [Route("logout")]
         public async Task<IActionResult> logout()
        {
            var result = "logout OK";
            return Ok(result);
        }

    }
}
