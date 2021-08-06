using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IApplication
    {
        string PublisherNamespace { get; set; }
        IPublisher Publisher {get; set;}
        string Namesspace {get; set;}
        string ShortName {get; set;}
        string Description {get; set;}
        IVersion Version {get; set;}

    }
}
