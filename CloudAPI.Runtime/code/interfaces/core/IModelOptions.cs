using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IModelOptions
    {
        int PagingFrom {get; set;}
        int PagingTo {get; set;}
        string SortOrder {get; set;}
        List<string> Expansion {get; set;}
        List<string> Filtering {get; set;}

    }
}
