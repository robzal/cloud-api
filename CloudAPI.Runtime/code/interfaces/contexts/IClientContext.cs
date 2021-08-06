using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IClientContext
    {
        string DeviceInfo {get; set;}
        string GeoLocation {get; set;}
        string Language {get; set;}
        string Culture {get; set;}
        int TimezoneOffset {get; set;}

    }
}
