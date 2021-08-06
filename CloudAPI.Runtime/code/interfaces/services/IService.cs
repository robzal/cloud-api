using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public interface IService
    {
        string EnvironmentName {get; set;}
        Scope Scope { get; set; }
        ServiceType ServiceType {get; set;}
        string Key {get; set;}
        string Alias {get; set;}
        ServiceStatus Status {get; set;}
        ServiceStatus Test ();
        ServiceStatus Start ();
        ServiceStatus Stop ();
        List<ISetting> Settings {get; set;}
        IServiceResult Result {get; set;}

    }

    public interface IServiceResult
    {
        int ResultCode {get; set;}
        string Message {get; set;}
        int RowsAffected {get; set;}
        List<string> ObjectIDs {get; set;} 
    }
}
