using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CloudAPI.Runtime
{
    [Route("api/host")]
    public class HostController : BaseController
    {

        public HostController(IHttpContextAccessor HttpContextAccessor) : base(HttpContextAccessor)
        {
        }
        
        [HttpGet]
        [Route("info")]
         public async Task<IActionResult> Info()
        {
            return Ok("Host info goes here.");
        }

    }
}
