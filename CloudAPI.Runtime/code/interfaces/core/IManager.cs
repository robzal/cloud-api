using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IManager
    {
        IRequestContext RequestContext {get; set;}
        IResult Result {get; set;}
   
    }
}
