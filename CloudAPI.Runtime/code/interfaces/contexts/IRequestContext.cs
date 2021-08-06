using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IRequestContext
    {
        IRequest Request {get; set;}
        ILogin Login {get; set;}
        IModel Model {get; set;}
        IModelOptions ModelOptions {get; set;}
    
        IApplicationContext ApplicationContext {get; set;}
        IUserContext UserContext {get; set;}
        IClientContext ClientContext {get; set;}

        IObjectContext GetObjectContext(string ObjectNamespace);
    }
}
