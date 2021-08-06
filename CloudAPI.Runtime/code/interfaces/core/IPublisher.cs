using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IPublisher
    {

        string PublisherNamespace { get; set; }
        string ShortName {get; set;}
        string Description {get; set;}
        
    }
}
