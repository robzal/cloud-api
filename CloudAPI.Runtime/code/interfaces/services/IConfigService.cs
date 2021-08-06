using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IConfigService
    {
        string GetConfig(string section, string key);
        object GetConfigSection(string section);

    }
}
