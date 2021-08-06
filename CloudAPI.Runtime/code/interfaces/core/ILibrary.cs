using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface ILibrary
    {
        string PublisherNamespace {get; set;}

        string LibraryNamespace {get; set;
        }
        string Platform {get; set;}

        string FullPath {get; set;}
    }
}
