using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CloudAPI.Runtime
{
    public class ApiExpressController<T> : BaseController
    {
        public ApiExpressController() : base() 
        {

        }
        public ApiExpressController(IHttpContextAccessor HttpContextAccessor) : base(HttpContextAccessor)
        {
            
        }

        [HttpGet]
        [Route("get")]
         public virtual async Task<IActionResult> Get(string id)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Get(id);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.Get(id)", string.Format ("Uncaught controller exception for  get request for type {0} for id {1}", typeof(T).FullName, id));
                throw new ApiExpressRequestException("ApiExpressController.Get(id)",string.Format("Uncaught controller exception for  get request for type {0} for id {1}", typeof(T).FullName, id));
            }
        }
        [HttpGet]
        [Route("getmulti")]
         public virtual async Task<IActionResult> Get([FromQuery] List<string> id)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Get(id);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.GetMulti(id)", string.Format ("Uncaught controller exception for  get request for type {0} for id {1}", typeof(T).FullName, id.ToString()));
                throw new ApiExpressRequestException("ApiExpressController.GetMulti(id)",string.Format("Uncaught controller exception for  get request for type {0} for id {1}", typeof(T).FullName, id.ToString()));
            }
        }
        [HttpGet]
        [Route("getall")]
         public virtual async Task<IActionResult> Get()
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Get();
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.Get", string.Format ("Uncaught controller exception for  get request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.Get",string.Format("Uncaught controller exception for  get request for type {0}", typeof(T).FullName));
            }
        }
        [HttpGet]
        [Route("getquery")]
         public virtual async Task<IActionResult> Get(IModelOptions query)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Get(query);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.GetQuery", string.Format ("Uncaught controller exception for  get request for type {0} using passed options", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.GetQuery",string.Format("Uncaught controller exception for  get request for type {0} using passed options", typeof(T).FullName));
            }
        }

        [HttpPost]
        [Route("add")]
         public virtual async Task<IActionResult> Add(T data)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Add(data);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.Add", string.Format ("Uncaught controller exception for  add request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.Add",string.Format("Uncaught controller exception for  add request for type {0}", typeof(T).FullName));
            }
        }
        [HttpPost]
        [Route("addmulti")]
         public virtual async Task<IActionResult> Add(List<T> data)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Add(data);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.AddMulti", string.Format ("Uncaught controller exception for  add request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.AddMulti",string.Format("Uncaught controller exception for  add request for type {0}", typeof(T).FullName));
            }
        }

        [HttpPut]
        [Route("edit")]
         public virtual async Task<IActionResult> Edit(T data)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Edit(data);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.Edit", string.Format ("Uncaught controller exception for  edit request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.Edit",string.Format("Uncaught controller exception for  edit request for type {0}", typeof(T).FullName));
            }
        }
        [HttpPut]
        [Route("editmulti")]
         public virtual async Task<IActionResult> Edit(List<T> data)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Edit(data);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.EditMulti", string.Format ("Uncaught controller exception for  edit request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.EditMulti",string.Format("Uncaught controller exception for  edit request for type {0}", typeof(T).FullName));
            }
        }

        [HttpDelete]
        [Route("delete")]
         public virtual async Task<IActionResult> Delete(string id)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Delete(id);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.Delete", string.Format ("Uncaught controller exception for  delete request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.Delete",string.Format("Uncaught controller exception for  delete request for type {0}", typeof(T).FullName));
            }
        }
        [HttpDelete]
        [Route("deletemulti")]
         public virtual async Task<IActionResult> Delete(List<string> id)
        {
            try
            {
                ApiExpressManager<T> mgr = new ApiExpressManager<T>(this.RequestContext);
                await mgr.Delete(id);
                return this.Handler.CreateResponse(mgr.Result.ResponseCode, mgr.Result.Data);
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressController.DeleteMulti", string.Format ("Uncaught controller exception for  delete request for type {0}", typeof(T).FullName));
                throw new ApiExpressRequestException("ApiExpressController.DeleteMulti",string.Format("Uncaught controller exception for  delete request for type {0}", typeof(T).FullName));
            }
        }

    }
}
