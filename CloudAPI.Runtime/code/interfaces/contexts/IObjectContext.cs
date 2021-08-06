using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IObjectContext
    {
    
        IModel Model {get; set;}
        List<IModel> RelatedModels { get; set; }
        List<IService> Services { get; set; }
        List<ISetting> Settings { get; set; }
        List<ILibrary> Libraries {get; set;}

    }
}
