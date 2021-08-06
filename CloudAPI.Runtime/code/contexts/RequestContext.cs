using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{   
    public class RequestContext : BaseObject, IRequestContext
    {
        public string Notes {get; set;}
        public IRequest Request {get; set;}
        public ILogin Login {get; set;}
        public IModel Model {get; set;}
        public IModelOptions ModelOptions {get; set;}
    
        public IApplicationContext ApplicationContext {get; set;}
        public IUserContext UserContext {get; set;}
        public IClientContext ClientContext {get; set;}

        public RequestContext() : base()
        {

        }
        public RequestContext (IRequest Request): this()
        {
                this.Request = Request;
        }

        public IObjectContext GetObjectContext(string ObjectNamespace)
        {
            // TODO build objectcontext
            return new ObjectContext();
        }

    }
}
