using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public class ApiExpressManager<T> : BaseManager
    {
        public List<T> Items { get; set; }
        public ApiExpressManager () : base() 
        {
            this.Items = new List<T>();
        } 
        public ApiExpressManager (IRequestContext RequestContext) : this() 
        {

            this.RequestContext = RequestContext;
        }

        public virtual async Task<T> Get(string id) 
        {
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();

                JSObject js =  await datasvc.Get<T>(m, id);
                //this assumes T will always implement IObject
                IObject newObject = js.Deserialize<T>();
                this.Items.Add((T) newObject);
                string msg = string.Format("Success. 1 row returned.");
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
                return (T) this.Items[0];
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Get", string.Format ("Couldnt get the data of type {0} for id {1}", typeof(T).FullName, id));
                throw new ApiExpressDataException("ApiExpressManager.Get",string.Format("Couldnt get the data of type {0} for id {1}", typeof(T).FullName, id));
            }
        }
        public virtual async Task<List<T>> Get(List<string> ids) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();

                JSObject js =  await datasvc.GetList<T>(m, ids);
                //this assumes T will always implement IObject
                List<IObject> newObject = js.DeserializeList<T>();
                this.Items = newObject.Cast<T>().ToList();
                string msg = string.Format("Success. {0} rows returned.", this.Items.Count);
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
                return (List<T>) this.Items;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Get", string.Format ("Couldnt get the data of type {0} for id {1}", typeof(T).FullName, ids.ToString()));
                throw new ApiExpressDataException("ApiExpressManager.Get",string.Format("Couldnt get the data of type {0} for id {1}", typeof(T).FullName, ids.ToString()));
            }

        }
        public virtual async Task<List<T>> Get() 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();

                JSObject js =  await datasvc.GetList<T>(m);
                //this assumes T will always implement IObject
                List<IObject> newObject = js.DeserializeList<T>();
                this.Items = newObject.Cast<T>().ToList();
                string msg = string.Format("Success. {0} rows returned.", this.Items.Count);
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
                return (List<T>) this.Items;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Get", string.Format ("Couldnt get the data of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Get",string.Format("Couldnt get the data of type {0}", typeof(T).FullName));
            }

        }
        public virtual async Task<List<T>> Get(IModelOptions query) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();

                JSObject js =  await datasvc.GetList<T>(m,query);
                //this assumes T will always implement IObject
                List<IObject> newObject = js.DeserializeList<T>();
                this.Items = newObject.Cast<T>().ToList();
                string msg = string.Format("Success. {0} rows returned.", this.Items.Count);
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
                return (List<T>) this.Items;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Get", string.Format ("Couldnt get the data of type {0} for modeloption", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Get",string.Format("Couldnt get the data of type {0} for id modeloption", typeof(T).FullName));
            }

        }
        public virtual async Task<T> Add(T obj) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();

                IServiceResult dr =  await datasvc.Insert<T>(m, (IObject) obj);
                string id = dr.ObjectIDs[0];
                JSObject js =  await datasvc.Get<T>(m, id);
                //this assumes T will always implement IObject
                IObject newObject = js.Deserialize<T>();
                this.Items.Add((T) newObject);
                string msg = string.Format("Success. 1 row affected.");
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items[0] };
                return (T) this.Items[0];
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Add", string.Format ("Couldnt add object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Add",string.Format("Couldnt add object of type {0}", typeof(T).FullName));
            }

        }
        public virtual async Task<List<T>> Add(List<T> objlist) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();
                List<string> ids = new List<string>();

                foreach(T obj in objlist)
                {
                    IServiceResult dr =  await datasvc.Insert<T>(m, (IObject) obj);
                    ids.Add(dr.ObjectIDs[0]);
                }

                JSObject js =  await datasvc.GetList<T>(m, ids);
                //this assumes T will always implement IObject
                List<IObject> newObject = js.DeserializeList<T>();
                this.Items = newObject.Cast<T>().ToList();
                string msg = string.Format("Success. {0} rows affected.", ids.Count);
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
                return (List<T>) this.Items;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Add", string.Format ("Couldnt add object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Add",string.Format("Couldnt add object of type {0}", typeof(T).FullName));
            }
        }
        public virtual async Task<T> Edit(T obj) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();
                IServiceResult dr =  await datasvc.Update<T>(m, (IObject) obj);

                string id = dr.ObjectIDs[0];
                JSObject js =  await datasvc.Get<T>(m, id);
                //this assumes T will always implement IObject
                IObject newObject = js.Deserialize<T>();
                this.Items.Add((T) newObject);
                string msg = string.Format("Success. 1 row affected.");
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items[0] };
                return (T) this.Items[0];
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Edit", string.Format ("Couldnt edit object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Edit",string.Format("Couldnt edit object of type {0}", typeof(T).FullName));
            }
        }
        public virtual async Task<List<T>> Edit(List<T> objlist) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();
                List<string> ids = new List<string>();

                foreach(T obj in objlist)
                {
                    IServiceResult dr =  await datasvc.Update<T>(m, (IObject) obj);
                    ids.Add(dr.ObjectIDs[0]);
                }

                JSObject js =  await datasvc.GetList<T>(m, ids);
                //this assumes T will always implement IObject
                List<IObject> newObject = js.DeserializeList<T>();
                this.Items = newObject.Cast<T>().ToList();
                string msg = string.Format("Success. {0} rows affected.", ids.Count);
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
                return (List<T>) this.Items;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Edit", string.Format ("Couldnt edit object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Edit",string.Format("Couldnt edit object of type {0}", typeof(T).FullName));
            }
        }
        public virtual async Task Delete(string id) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();
                DataResult dr = (DataResult) await datasvc.Delete<T>(m,id);
                string msg = string.Format("Success. 1 row affected.");
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items[0] };
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Delete", string.Format ("Couldnt delete object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Delete",string.Format("Couldnt delete object of type {0}", typeof(T).FullName));
            }            
        }
        public virtual async Task Delete(List<string> ids) 
        { 
            try
            {
                IModel m = GetModel();
                IDataService datasvc = GetDataService();
                DataResult dr = (DataResult) await datasvc.DeleteList<T>(m,ids);
                string msg = string.Format("Success. {0} rows affected.", dr.RowsAffected);
                this.Result = new Result {ResponseCode = 200, Message = msg, Data = this.Items };
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.Delete", string.Format ("Couldnt delete object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("ApiExpressManager.Delete",string.Format("Couldnt delete object of type {0}", typeof(T).FullName));
            }
        }
       
        private IDataService GetDataService()
        {
            try
            {
                // TODO find appropriate services in .RequestContext, which also has the model in it
                IDataService datasvc = (IDataService) this.RequestContext.ApplicationContext.Services.First(i => i.ServiceType == ServiceType.DBSTORAGE);
                return datasvc;            
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.GetDataService", string.Format ("Couldnt get the DataService to perform the action"));
                throw new ApiExpressDataException("ApiExpressManager.GetDataService",string.Format("Couldnt get the DataService to perform the action"));
            }
        }

        private IModel GetModel()
        {
            try
            {
                IModel m = this.RequestContext.Model;
                return m;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressManager.GetModel", string.Format ("Couldnt get the Model to perform the action"));
                throw new ApiExpressDataException("ApiExpressManager.GetModel",string.Format("Couldnt get the Model to perform the action"));
            }
        }
        
    }
}
