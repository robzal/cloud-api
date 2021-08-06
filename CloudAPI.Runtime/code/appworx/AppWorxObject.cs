using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{

    public class ApiExpressObject : BaseObject, IObject
    {

        public Dictionary<string,bool> expandList;
        public bool ShouldSerializeexpandList() { return false; }
        public bool expandObject(string key)
        {
            if (expandList.ContainsKey(key)) { return expandList[key]; } else {return false;}
        }
        public ApiExpressObject() : base() 
        {
            this.expandList = new Dictionary<string, bool>();
        }
        public virtual bool Validate() 
        {
            try
            {
                List<IModelRule> _rules = this.GetObjectContext().Model.Rules;
                foreach(IModelRule mr in _rules)
                {
                    // TODO base object field validation, remaining types
                    if (new [] {RuleType.FIELDTYPE, RuleType.FIELDREGEX}.Contains(  mr.RuleType))
                    {
                        string val = this.GetValue(mr.FieldName);
                        //test regex
                        Log(LogLevel.INFO,"ApiExpressObject.Validate",string.Format("Validating Rule {0} for Field {1}",mr.Name, mr.FieldName));
                        Regex rgx = new Regex(mr.RuleValue, RegexOptions.IgnoreCase);
                        MatchCollection matches = rgx.Matches(val);
                        if (matches.Count == 0)
                        {
                            Log(LogLevel.WARNING,"ApiExpressObject.Validate",string.Format("Validating Rule {0} for Field {1} failed. Field Value is {2}",mr.Name, mr.FieldName, val));
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressObject.Validate", "Unexpected error occured validating object");
                throw new ApiExpressModelException("ApiExpressObject.Validate","Unexpected error occured validating object"); 
            }
        }
        public virtual string GetValue (string fieldName) 
        {
            try 
            {
                PropertyInfo[] pp = this.GetType().GetProperties();
                string val = null;
                bool found = false;
                for (int i = 0; i < pp.Length; i++)
                {
                    if (((PropertyInfo) pp[i]).Name.ToUpper() == fieldName.ToUpper())
                    {
                        found = true;
                        if (pp[i].GetValue(this) != null)
                        {
                            val = pp[i].GetValue(this).ToString();
                        }
                    }
                }
                if (found == false) 
                {
                    this.Log(LogLevel.ERROR, "ApiExpressObject.GetValue", string.Format ("Couldnt get the {0} property of the object", fieldName));
                    throw new ApiExpressDataException("ApiExpressObject.GetValue",string.Format("Couldnt get the {0} property of the object", fieldName));
                }
                return val;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressObject.GetValue", string.Format ("Couldnt get the {0} property of the object", fieldName));
                throw new ApiExpressDataException("ApiExpressObject.GetValue",string.Format("Couldnt get the {0} property of the object", fieldName));
            }
        }
        public virtual void SetValue(string fieldName, object fieldValue) 
        {
            try 
            {
                PropertyInfo[] pp = this.GetType().GetProperties();
                bool found = false;
                for (int i = 0; i < pp.Length; i++)
                {
                    if (((PropertyInfo) pp[i]).Name.ToUpper() == fieldName.ToUpper())
                    {
                        found = true;
                        if (fieldValue != null)
                        {
                            pp[i].SetValue(this, fieldValue, null);
                        }
                    }
                }
                if (found == false) 
                {
                    this.Log(LogLevel.ERROR, "ApiExpressObject.SetValue", string.Format ("Couldnt set the {0} property of the object", fieldName));
                    throw new ApiExpressDataException("ApiExpressObject.SetValue",string.Format("Couldnt set the {0} property of the object", fieldName));
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressObject.SetValue", string.Format ("Couldnt set the {0} property of the object", fieldName));
                throw new ApiExpressDataException("ApiExpressObject.SetValue",string.Format("Couldnt set the {0} property of the object", fieldName));
            }
        }
        public virtual FieldType GetFieldType(string fieldName) 
        {
            try 
            {
                PropertyInfo[] pp = this.GetType().GetProperties();
                FieldType ft = Runtime.FieldType.UNKNOWN;
                for (int i = 0; i < pp.Length; i++)
                {
                    if (((PropertyInfo) pp[i]).Name.ToUpper() == fieldName.ToUpper())
                    {
                        Type t = pp[i].GetType();
                        //convert to fieldtype
                        switch (t.Name.ToLower())
                        {
                            case "string":
                                ft = Runtime.FieldType.STRING;
                                break;
                            case "char":
                                ft = Runtime.FieldType.CHAR;
                                break;
                            case "bool":
                                ft = Runtime.FieldType.BOOLEAN;
                                break;
                            case "boolean":
                                ft = Runtime.FieldType.BOOLEAN;
                                break;
                            case "byte":
                                ft = Runtime.FieldType.BYTE;
                                break;
                            case "int":
                                ft = Runtime.FieldType.INT;
                                break;
                            case "long":
                                ft = Runtime.FieldType.LONG;
                                break;
                            case "float":
                                ft = Runtime.FieldType.FLOAT;
                                break;
                            case "double":
                                ft = Runtime.FieldType.DOUBLE;
                                break;
                            case "decimal":
                                ft = Runtime.FieldType.DECIMAL;
                                break;
                            case "datetime":
                                ft = Runtime.FieldType.DATETIME;
                                break;
                            case "object":
                                ft = Runtime.FieldType.OBJECT;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (ft == Runtime.FieldType.UNKNOWN) 
                {
                    this.Log(LogLevel.ERROR, "ApiExpressObject.GetFieldType", string.Format ("Couldnt determine fieldtype of field {0}", fieldName));
                    throw new ApiExpressModelException("ApiExpressObject.GetFieldType",string.Format ("Couldnt determine fieldtype of field {0}", fieldName));                
                }
                return ft;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "ApiExpressObject.GetFieldType", string.Format ("Couldnt determine fieldtype of field {0}", fieldName));
                throw new ApiExpressModelException("ApiExpressObject.GetFieldType",string.Format ("Couldnt determine fieldtype of field {0}", fieldName));
            }
        }

    }
}
