using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class Result : BaseObject, IResult
    {
        public int ResponseCode {get; set;}
        public string ResultCode {get; set;}
        public string Error {get; set;}
        public string Message {get; set;}
        public object Data {get; set;}
    }
}
