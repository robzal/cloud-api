using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{   
    public class ObjectContext : BaseObject, IObjectContext
    {
        public IModel Model {get; set;}
        public List<IModel> RelatedModels { get; set; }
        public List<IService> Services { get; set; }
        public List<ISetting> Settings { get; set; }
        public List<ILibrary> Libraries { get; set; }

        public ObjectContext() : base()
        {
            this.RelatedModels = new List<IModel>();
            this.Services = new List<IService>();
            this.Settings = new List<ISetting>();
            this.Libraries = new List<ILibrary>();
        }
    }
}
