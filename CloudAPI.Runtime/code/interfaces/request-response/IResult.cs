using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IResult
    {
        int ResponseCode {get; set;}
        string ResultCode {get; set;}
        string Error {get; set;}
        string Message {get; set;}
        object Data {get; set;}

    }
}
