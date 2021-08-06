using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CloudAPI.Runtime
{
    public class RequestHandler : BaseObject, IRequestHandler
    {
        public Object NativeRequest {get; set;}
        public IRequest Request {get; set;}
        public IRequestContext RequestContext{get; set;}
        public void GetRequest(Object nativeRequest)
        {
            try
            {
                this.NativeRequest = nativeRequest;
                // TODO parse the native request into an ApiExpress request
                this.Request = new Request();
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "RequestHandler.GetRequest", "Could not parse the HTTP request.");
                throw new ApiExpressRequestException("RequestHandler.GetRequest", "Could not parse the HTTP request.");
            }
            
        }
        public void GetContext () 
        {
            try 
            {
                if (this.Request != null) 
                {
                    // TODO fill the RequestContext from the request
                    RequestContext rc = new RequestContext {Notes = "Still need to populate the request RequestContext" };
                    ApplicationContext a = new ApplicationContext();
                    ClientContext cc = new ClientContext();
                    rc.ClientContext = cc;
                    rc.ApplicationContext = a;
                    Model m = new Model("ApiExpress.CoffeeRun","CoffeeRun");
                    m.Fields.Add(new ModelField() {Name = "ID",DataType = FieldType.INT,MaxLength = 0,IsRequired = true ,IsPrimaryKey = true});
                    m.Fields.Add(new ModelField("userID",FieldType.STRING,0,false));
                    m.Fields.Add(new ModelField("Location",FieldType.STRING,0,false));
                    m.Rules.Add(new ModelRule("LocationCheck",RuleType.RECORDFUNCTION, "RuleValue", null));
                    m.Rules.Add(new ModelRule("Location",RuleType.FIELDTYPE, "RuleValue", "Location"));
                    ModelOptions mo = new ModelOptions();
                    rc.Model = m;
                    rc.ModelOptions = mo;
                    List<IService> svcs = new List<IService>();
                    DataService data = new DataService();
                    data.Provider = DataProvider.MYSQL;
                    data.ConnectionString = "server=localhost;SslMode=None;user id=apiexpress;password=apiexpress;persistsecurityinfo=True;port=3306;database=coffeerun";
                    LogService log = new LogService();
                    AuthService auth = new AuthService();
                    ConfigService conf = new ConfigService();
                    svcs.Add(data);
                    svcs.Add(log);
                    svcs.Add(auth);
                    svcs.Add(conf);
                    a.Services = svcs;
                    this.RequestContext = rc;
                }
                else
                {
                    throw new ApiExpressRequestException ( "RequestHandler.GetContext", "Could not get the RequestContext from the HTTP request.");
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "RequestHandler.GetContext", "Could not get the RequestContext from the HTTP request.");
                throw new ApiExpressRequestException("RequestHandler.GetContext", "Could not get the RequestContext from the HTTP request.");
            }


        }
        public IActionResult CreateResponse(int ResponseCode, Object Data) 
        {
            switch (ResponseCode)
            {
                // TODO construct all responses
                case 200:
                    return new OkObjectResult(Data);
                case 401:
                    return new UnauthorizedResult();
                case 403:
                    return new ForbidResult();
                case 404:
                    return new NotFoundResult();
                case 500:
                    return new BadRequestResult();

                default:
                    return new BadRequestResult();
            }
        }

    }
}
