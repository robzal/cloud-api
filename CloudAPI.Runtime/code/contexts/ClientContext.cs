using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class ClientContext : BaseObject, IClientContext
    {
        public string DeviceInfo {get; set;}
        public string GeoLocation {get; set;}
        public string Language {get; set;}
        public string Culture {get; set;}
        public int TimezoneOffset {get; set;}

        public ClientContext() : base()
        {
            
        }
        
    }
}
