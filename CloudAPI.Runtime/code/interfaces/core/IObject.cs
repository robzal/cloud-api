using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public interface IObject
    {
        bool Validate(); 
        string GetValue(string fieldName);
        void SetValue(string fieldName, object fieldValue);
        FieldType GetFieldType (string fieldName);
    }
}
