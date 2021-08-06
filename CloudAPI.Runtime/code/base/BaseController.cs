using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CloudAPI.Runtime
{
    public abstract class BaseController : Controller
    {
        public new IRequest Request {get; set;}
        public IRequestContext RequestContext {get; set;}
        protected IRequestHandler Handler {get; set;}

        public BaseController() : base()
        {
            
        }
        public BaseController(IHttpContextAccessor HttpContextAccessor): this()
        {
            try
            {
                string customHandler = this.Config("Application", "RequestHandlerLibrary");
                if (! string.IsNullOrWhiteSpace(customHandler))
                {
                    this.Log(LogLevel.INFO, "BaseController.ctor", string.Format("Loading custom RequestHandler library {0}", customHandler));
                    this.Handler = (IRequestHandler) Activator.CreateInstance(Type.GetType(customHandler));                
                }
                else
                {
                    this.Log(LogLevel.INFO, "BaseController.ctor", string.Format("Loading standard RequestHandler library"));
                    this.Handler = new RequestHandler();
                }
                var req = HttpContextAccessor.HttpContext.Request;
                this.Handler.GetRequest(req);
                this.Handler.GetContext();
                this.Request = Handler.Request;
                this.RequestContext = Handler.RequestContext;      
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "BaseController.ctor", string.Format("Couldnt start the BaseController"));
                throw new ApiExpressRequestException("BaseController.ctor",string.Format("Couldnt start the BaseController"));
            }
            
        }

        protected void Log(LogLevel level, string source, string message)
        {
            try
            {
                try
                {
                    ILogService logsvc = (ILogService) this.RequestContext.UserContext.Services.First(i => i.ServiceType == ServiceType.LOG);
                    logsvc.LogMessage(level, source, message);
                }
                catch
                {
                    try
                    {
                        ILogService logsvc = (ILogService) this.RequestContext.ApplicationContext.Services.First(i => i.ServiceType == ServiceType.LOG);
                        logsvc.LogMessage(level, source, message);
                    }
                    catch
                    {
                        // plan C use standard log service, which will just output to console
                        ILogService logsvc = new LogService();
                        logsvc.LogMessage(level, source, message);                        
                    }
                }
            }
            catch
            {
                // make sure to catch any errors queitly otherwise we will potentially have infinitely looping in the callers who log exceptions!
                string logMessage = string.Format("{0} - Error Logging Message for {1} - {2}", DateTime.Now.ToString(), source, message);
                Console.WriteLine (logMessage);

            }
        }
        protected string Config(string section, string key)
        {
            try
            {
                try
                {
                    IConfigService configsvc = (IConfigService) this.RequestContext.UserContext.Services.First(i => i.ServiceType == ServiceType.CONFIGURATION);
                    string config = configsvc.GetConfig(section, key);
                    return config;
                }
                catch
                {
                    try
                    {
                        IConfigService configsvc = (IConfigService) this.RequestContext.ApplicationContext.Services.First(i => i.ServiceType == ServiceType.CONFIGURATION);
                        string config = configsvc.GetConfig(section, key);
                        return config;
                    }
                    catch
                    {
                        // plan C - use standard Config Service, with defaults
                        IConfigService configsvc = new ConfigService();
                        string config = configsvc.GetConfig(section, key);
                        return config;
                    }
                }
            }
            catch
            {
                Log(LogLevel.ERROR,"BaseController.Config", string.Format("Error retrieving config setting for section {0} - key {1}", section, key));
                throw new ApiExpressRequestException("BaseController.Config", string.Format("Error retrieving config setting for section {0} - key {1}", section, key));
            }
        }
        
    }
}
