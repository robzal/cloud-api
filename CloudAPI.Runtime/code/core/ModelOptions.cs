using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class ModelOptions : BaseObject, IModelOptions
    {
        public int PagingFrom {get; set;}
        public int PagingTo {get; set;}
        public string SortOrder {get; set;}
        public List<string> Expansion {get; set;}
        public List<string> Filtering {get; set;}

        public ModelOptions() : base()
        {
            this.PagingFrom = 0;
            this.PagingTo = 0;
            this.Expansion = new List<string>();
            this.Filtering = new List<string>();
        }
    }
}
