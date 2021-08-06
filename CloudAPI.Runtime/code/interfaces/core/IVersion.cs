using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IVersion
    {
        string Number {get; set;}
        string Name {get; set;}
        int BuildNumber {get ;set;}
        DateTime BuildDate {get; set;}
        string BuildNotes {get; set;} 
    }
}
