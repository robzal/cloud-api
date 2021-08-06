using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using CloudAPI.Runtime;

namespace CloudAPI.Utils
{
    public class JSObject : BaseObject
    {
        public string jsondata {get; set;}
        public bool isArray {get; set;}
        public JSObject ()
        {
            this.isArray = false;
        }
        public JSObject (string jsonData): this()
        {
            this.Parse(jsonData);
        }
        public IObject Deserialize<T> ()
        {
            T obj;
            try 
            {
                obj = JsonConvert.DeserializeObject<T>(this.jsondata);
                return (IObject) obj;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "JSObject.Deserialize", string.Format ("Couldnt deserialize object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("JSObject.Deserialize",string.Format("Couldnt deserialize object of type {0}", typeof(T).FullName));
            }
        }

        public List<IObject> DeserializeList<T> ()
        {
            List<T> obj;
            try 
            {
                obj = JsonConvert.DeserializeObject<List<T>>(this.jsondata);
                return obj.Cast<IObject>().ToList();
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "JSObject.DeserializeList", string.Format ("Couldnt deserialize list of objects of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("JSObject.DeserializeList",string.Format("Couldnt deserialize list of object of type {0}", typeof(T).FullName));
            }
        }

        public string Serialize<T>( IObject obj) 
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                this.jsondata = json;
                return json;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "JSObject.Serialize", string.Format ("Couldnt serialize object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("JSObject.Serialize",string.Format("Couldnt serialize object of type {0}", typeof(T).FullName));
            }
        }

        public string SerializeList<T>( List<IObject> objlist) 
        {
            try
            {
                string json = JsonConvert.SerializeObject(objlist, Formatting.Indented);
                this.jsondata = json;
                return json;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "JSObject.SerializeList", string.Format ("Couldnt serialize list of object of type {0}", typeof(T).FullName));
                throw new ApiExpressDataException("JSObject.SerializeList",string.Format("Couldnt serialize list of object of type {0}", typeof(T).FullName));
            }
        }

        public void Parse (string jsonData)
        {
            try 
            {
                JObject.Parse(jsonData);
                this.jsondata = jsonData;            
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                try
                {
                    JArray.Parse(jsonData);
                    this.jsondata = jsonData;
                    this.isArray = true;          
                }
                catch (ApiExpressException awex2)
                {
                    throw awex2;
                }
                catch (Exception ex2)
                {
                    this.Log(LogLevel.ERROR, "JSObject.Parse", string.Format ("Couldnt parse json data - {0}", this.jsondata));
                    throw new ApiExpressDataException("JSObject.Parse",string.Format("Couldnt parse json data - {0}", this.jsondata));
                }
            }
        }
    }
}