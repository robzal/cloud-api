using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class Version : BaseObject, IVersion
    {
        public string Number {get; set;}
        public string Name {get; set;}
        public int BuildNumber {get ;set;}
        public DateTime BuildDate {get; set;}
        public string BuildNotes {get; set;} 
    }
}
