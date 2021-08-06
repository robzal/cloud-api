using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class Application : BaseObject, IApplication
    {
        public string PublisherNamespace { get; set; }
        public IPublisher Publisher {get; set;}
        public string Namesspace {get; set;}
        public string ShortName {get; set;}
        public string Description {get; set;}
        public IVersion Version {get; set;}
        public Application() : base()
        {

        }
    }
}
