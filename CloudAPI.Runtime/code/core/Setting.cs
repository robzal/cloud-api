using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class Setting : BaseObject, ISetting
    {
        public Scope Scope { get; set; }
        public string Namespace { get; set; }
        public string Key { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
    }
}
