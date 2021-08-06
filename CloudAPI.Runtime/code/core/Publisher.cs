using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class Publisher : BaseObject, IPublisher
    {
        public string PublisherNamespace { get; set; }
        public string ShortName {get; set;}
        public string Description {get; set;}
                
    }
}
