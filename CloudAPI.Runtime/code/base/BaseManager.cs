using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public abstract class BaseManager : BaseObject, IManager
    {
        public IRequestContext RequestContext {get; set;}
        public IResult Result {get; set;}

        public BaseManager() : base()
        {
            this.Result = new Result();
        }
        public BaseManager( IRequestContext RequestContext): this() 
        { 
            this.RequestContext = RequestContext; 
        }
        
    }
}
