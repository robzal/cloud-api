using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IUserContext
    {
        IPublisher Publisher {get; set;}
        IApplication Application {get; set;}
        ITenant Tenant {get; set;}
        IUser User {get; set;}
    
    
        List<IModel> Models { get; set; }
        List<IService> Services { get; set; }
        List<ISetting> Settings { get; set; }
        List<ILibrary> Libraries {get; set;}

    }
}
