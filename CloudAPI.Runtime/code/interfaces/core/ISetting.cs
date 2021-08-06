using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface ISetting
    {
        Scope Scope { get; set; }
        string Namespace { get; set; }
        string Key { get; set; }
        string SettingName { get; set; }
        string Value { get; set; }
    }
}
