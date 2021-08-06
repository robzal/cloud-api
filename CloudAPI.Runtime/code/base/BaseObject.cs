using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public abstract class BaseObject
    {
        private IObjectContext _context;
        public IObjectContext GetObjectContext()
        {
            return this._context;
        }
        public void SetObjectContext(IObjectContext objectContext)
        {
            this._context = objectContext;
        }
        public bool ShouldSerializeObjectContext() { return false; }

        public BaseObject() 
        {
        }
        public BaseObject( IObjectContext ObjectContext): this() 
        { 
            this.SetObjectContext (ObjectContext); 
        }
        
        public void Log(LogLevel level, string source, string message)
        {
            try
            {
                ILogService logsvc = (ILogService) this.GetObjectContext().Services.First(i => i.ServiceType == ServiceType.LOG);
                logsvc.LogMessage(level, source, message);
            }
            catch
            {
                // make sure to catch any errors queitly otherwise we will potentially have infinitely looping in the callers who log exceptions!
                try
                {
                    // plan B use standard log service, which will just output to console
                    ILogService logsvc = new LogService();
                    logsvc.LogMessage(level, source, message);                        
                }
                catch
                {
                    string logMessage = string.Format("{0} - Error Logging Message for {1} - {2}", DateTime.Now.ToString(), source, message);
                    Console.WriteLine (logMessage);
                }
            }
        }
        public string Config(string section, string key)
        {
            try
            {
                try
                {
                    IConfigService configsvc = (IConfigService) this.GetObjectContext().Services.First(i => i.ServiceType == ServiceType.CONFIGURATION);
                    string config = configsvc.GetConfig(section, key);
                    return config;
                }
                catch
                {
                    // plan B - use standard Config Service, with defaults
                    IConfigService configsvc = new ConfigService();
                    string config = configsvc.GetConfig(section, key);
                    return config;
                }
            }
            catch
            {
                Log(LogLevel.ERROR,"BaseObject.Config", string.Format("Error retrieving config setting for section {0} - key {1}", section, key));
                throw new ApiExpressServiceException("BaseObject.Config", string.Format("Error retrieving config setting for section {0} - key {1}", section, key));
            }
        }
    }
}
