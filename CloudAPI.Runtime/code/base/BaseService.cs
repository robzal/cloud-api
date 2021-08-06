using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public abstract class BaseService : BaseObject, IService
    {

        private List<Setting> _settings;
        public string EnvironmentName {get; set;}

        public BaseService() : base()
        {
            this.Status = ServiceStatus.RUNNING;
            this.EnvironmentName = "PRODUCTION";
            this._settings = new List<Setting>();
        }

        public virtual Scope Scope { get; set; }
        public virtual ServiceType ServiceType {get; set;}
        public virtual string Key {get; set;}
        public virtual string Alias {get; set;}
        public virtual ServiceStatus Status {get; set;}
        public virtual ServiceStatus Test () {
            return this.Status;
        }
        public virtual ServiceStatus Start () {
            this.Status = ServiceStatus.RUNNING;
            return this.Status;
        }
        public virtual ServiceStatus Stop () {
            this.Status = ServiceStatus.STOPPED;
            return this.Status;

        }
        public List<ISetting> Settings {get; set;}
        public IServiceResult Result {get; set;}

    }


}
