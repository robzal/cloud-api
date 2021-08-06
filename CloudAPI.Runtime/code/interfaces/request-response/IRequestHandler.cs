using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace CloudAPI.Runtime
{
    public interface IRequestHandler
    {
        Object NativeRequest {get; set;}
        IRequest Request {get; set;}
        IRequestContext RequestContext {get; set;}

        void GetRequest (Object nativeRequest);
        void GetContext();
        IActionResult CreateResponse(int ResponseCode, Object Data);
    }
}
