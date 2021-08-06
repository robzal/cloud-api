using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class Model : BaseObject, IModel
    {
        /*
        A few behaviours
        VerboseName = Name if not given
        PluralName = Name + s if not given
        DBTableName = Name if not given
        SortOrder = PK[0] if not given
        xxxSQL commands = if customSQL provided, use it, else construct it from the model
        SortOrder = PK field(s) if not given, else none

        defaults
        DBSchemaName = dbo
        IsManaged = true
         */
        private string _verboseName;
        private string _pluralName;
        private string _DBName;
        private List<IModelRule> _rules;
        private object sqldata;

        public Scope Scope { get; set; }
        public string ScopeField {get; set;}
        public string ScopeFilter {get; set;}
        public string PublisherNamespace { get; set; }
        public string Namespace { get; set; }
        public string Name {get; set;}
        public string VerboseName 
        {
            get { if (string.IsNullOrWhiteSpace(_verboseName)) { return this.Name;} else { return this._verboseName; } }
            set { this._verboseName = value; }
        }
        public string PluralName 
        {
            get { if (string.IsNullOrWhiteSpace(_pluralName)) { return this.Name + 's';} else { return this._pluralName; } }
            set { this._pluralName = value; }
        }
        public string Description {get; set;}
        public bool IsManaged {get; set;}
        public string DBTableName 
        {
            get { if (string.IsNullOrWhiteSpace(_DBName)) { return this.Name;} else { return this._DBName; } }
            set { this._DBName = value; }
        }
        public string DBSchemaName {get; set;}
        public string SortOrder {get; set;}
        public string CustomGetSQL {get; set;}
        public string CustomInsertSQL {get; set;}
        public string CustomUpdateSQL {get; set;}
        public string CustomDeleteSQL {get; set;}
        public List<IModelField> Fields { get; set; }
        public List<IModelFieldMapping> Mappings 
        { 
            get
            { 
                List<IModelFieldMapping> maps = new List<IModelFieldMapping>();
                foreach(IModelField f in this.Fields)
                {
                    IModelFieldMapping map = new ModelFieldMapping() {PropertyName = f.Name, FieldName = f.DBFieldName,  FieldType = f.DataType , IsRequired = f.IsRequired};
                    maps.Add(map);
                }
                return maps;
            }
        }
        public List<string> KeyFields 
        {
            get
            {
                List<string> keyfields = new List<string>();

                foreach(IModelField f in this.Fields)
                {
                    if (f.IsPrimaryKey)
                    {
                        keyfields.Add(f.Name);
                    }
                }
                return keyfields;
            }
        }
        public List<IModelRelationship> Relationships { get; set; }
        public List<IModelRule> Rules 
        { 
            get
            { 
                //now add the implicit rules in case they are not in there
                foreach(IModelField f in this.Fields)
                {
                    if (!f.IsCalculated)
                    {
                        // check if any constraints to the field exist
                        //IsRequired
                        //length
                        //unique
                        //field regext
                        if (f.IsRequired && !this._rules.Exists(x=>x.Name == string.Format("_{0}_1",f.Name)))
                        {
                            this._rules.Add(new ModelRule(string.Format("_{0}_1",f.Name),RuleType.FIELDTYPE,"^.{1,}$",f.Name));
                        }
                        if (f.IsUnique && !this._rules.Exists(x=>x.Name == string.Format("_{0}_2",f.Name)))
                        {
                            this._rules.Add(new ModelRule(string.Format("_{0}_2",f.Name),RuleType.DATAUNIQUE,"",f.Name));
                        }
                        if ((f.MinLength > 0 || f.MaxLength > 0) && !this._rules.Exists(x=>x.Name == string.Format("_{0}_3",f.Name)))
                        {
                            this._rules.Add(new ModelRule(string.Format("_{0}_3",f.Name),RuleType.FIELDTYPE,string.Format("^.{{{0},{1}}$",f.MinLength,f.MaxLength),f.Name));
                        }
                        if (!string.IsNullOrWhiteSpace(f.RuleFormula) && !this._rules.Exists(x=>x.Name == string.Format("_{0}_4",f.Name)))
                        {
                            this._rules.Add(new ModelRule(string.Format("_{0}_4",f.Name),RuleType.FIELDREGEX,f.RuleFormula,f.Name));
                        }
                    }

                }
                return this._rules;
            }
            set
            {
                this._rules = value;
            }
        }
        public List<IService> Services { get; set; }
        public List<ISetting> Settings { get; set; }
        public List<ILibrary> Libraries { get; set; }
        public List<IModel> RelatedModels {get; set;}
        public IModelOptions Options {get; set;}

        public Model() : base()
        {
            this.IsManaged = true;
            this.Fields = new List<IModelField>();
            this.Relationships = new List<IModelRelationship>();
            this._rules = new List<IModelRule>();
            this.Services = new List<IService>();
            this.Settings = new List<ISetting>();
            this.Libraries = new List<ILibrary>();
            this.RelatedModels = new List<IModel>();
            this.Options = new ModelOptions();

        }

        public Model (string Namespace, string Name) : this() 
        {
            this.Namespace = Namespace;
            this.Name = Name;
        }

        public Model (string Namespace, string Name, bool IsManaged) : this() 
        {
            this.Namespace = Namespace;
            this.Name = Name;
            this.IsManaged = IsManaged;
        }
        
        public string GetSQL () 
        {
            try
            {
                string sql = "";
                sql = string.Format("{0} where 1 = 1 {1} {2}", SQLbuilder("BaseGetSQL"), SQLbuilder("ScopeFilter"), SQLbuilder("SortOrder")); 
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.GetSQL", string.Format ("Couldnt build the SQL get statement"));
                throw new ApiExpressModelException("Model.GetSQL",string.Format("Couldnt build the SQL get statement"));
            }
        }
        public string GetSQL (List<string> ids) 
        {
            try
            {
                this.sqldata = ids;
                string sql = string.Format("{0} where 1 = 1 {1} {2} {3}", SQLbuilder("BaseGetSQL"), SQLbuilder("ScopeFilter"), SQLbuilder("DataFilter"), SQLbuilder("SortOrder")); 
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.GetSQL", string.Format ("Couldnt build the SQL get statement using Ids {0}", ids.ToString()));
                throw new ApiExpressModelException("Model.GetSQL",string.Format("Couldnt build the SQL get statement using Ids {0}", ids.ToString()));
            }
        }
        public string GetSQL (IModelOptions options) 
        { 
            try
            {
                this.sqldata = options;
                string sql = string.Format("{0} where 1 = 1 {1} {2} {3}", SQLbuilder("BaseGetSQL"), SQLbuilder("ScopeFilter"), SQLbuilder("DataFilter"), SQLbuilder("SortOrder")); 
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.GetSQL", string.Format ("Couldnt build the SQL get statement using modeloptions"));
                throw new ApiExpressModelException("Model.GetSQL",string.Format("Couldnt build the SQL get statement using modeloptions"));
            }        
        }
        public string InsertSQL (IObject obj) 
        { 
            try
            {
                //this function is performed in 2 steps
                this.sqldata = obj;
                //first build the template statement
                string sql = string.Format("{0}", SQLbuilder("BaseInsertSQL")); 
                //second fill in the object properties
                sql = SQLTemplateFill(sql, obj);
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.InsertSQL", string.Format ("Couldnt build the SQL insert statement"));
                throw new ApiExpressModelException("Model.InsertSQL",string.Format("Couldnt build the SQL insert statement"));
            }        
        }
        public string UpdateSQL(IObject obj) 
        {
            try
            {
                //this function is performed in 2 steps
                this.sqldata = obj;
                //first build the template statement with scopefilter just in case
                string sql = string.Format("{0} where 1 = 1 {1} {2}", SQLbuilder("BaseUpdateSQL"), SQLbuilder("ScopeFilter"), SQLbuilder("ObjectFilter")); 
                //second fill in the object properties
                sql = SQLTemplateFill(sql, obj);
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.UpdateSQL", string.Format ("Couldnt build the SQL update statement"));
                throw new ApiExpressModelException("Model.UpdateSQL",string.Format("Couldnt build the SQL update statement"));
            }        
        }
        public string DeleteSQL() 
        {
            try
            {
                string sql = string.Format("{0} where 1 = 1 {1}", SQLbuilder("BaseDeleteSQL"), SQLbuilder("ScopeFilter")); 
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.DeleteSQL", string.Format ("Couldnt build the SQL delete statement"));
                throw new ApiExpressModelException("Model.DeleteSQL",string.Format("Couldnt build the SQL delete statement"));
            }        
        }
        public string DeleteSQL(List<string> ids) 
        {
            try
            {
                this.sqldata = ids;
                string sql = string.Format("{0} where 1 = 1 {1} {2}", SQLbuilder("BaseDeleteSQL"), SQLbuilder("ScopeFilter"), SQLbuilder("DataFilter")); 
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.DeleteSQL", string.Format ("Couldnt build the SQL delete statement for IDs {0}", ids.ToString()));
                throw new ApiExpressModelException("Model.DeleteSQL",string.Format("Couldnt build the SQL delete statement for IDs {0}", ids.ToString()));
            }        
        }

        private string SQLTemplateFill(string sql, IObject obj)
        {
            try
            {
                foreach(IModelField f in this.Fields)
                {
                    if (obj.GetValue(f.Name) == null)
                    {
                        sql = sql.Replace(string.Format("{{{{{0}}}}}", f.Name), "null");
                    }
                    else if (f.DataType == FieldType.STRING || f.DataType == FieldType.DATETIME || f.DataType == FieldType.CHAR)
                    {
                        sql = sql.Replace(string.Format("{{{{{0}}}}}", f.Name), string.Format("'{0}'", obj.GetValue(f.Name)));
                    }
                    else
                    {
                        sql = sql.Replace(string.Format("{{{{{0}}}}}", f.Name), string.Format("{0}", obj.GetValue(f.Name)));
                    }

                }
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.SQLTemplateFill", string.Format ("Couldnt fill the SQL template with fieldnames or values - {0}", sql));
                throw new ApiExpressModelException("Model.SQLTemplateFill",string.Format("Couldnt fill the SQL template with fieldnames or values - {0}", sql));
            }        
        }

        private string SQLFieldSQL(string fieldName, List<string> values = null)
        {
            try 
            {
                //dynamically constructs a SQL field assignment snippet
                string sql = "";
                FieldType keytype = GetFieldType(fieldName);

                if (values == null)
                {
                    sql = string.Format ("{{{{{0}}}}}", fieldName);
                }
                else if (values.Count == 1)
                {
                    //return a single field assignment statement
                    if (keytype == FieldType.STRING || keytype == FieldType.DATETIME || keytype == FieldType.CHAR)
                    {
                        sql = string.Format ("{0} = '{1}'", fieldName, values[0]);
                    }
                    else
                    {
                        sql = string.Format ("{0} = {1}", fieldName, values[0]);
                    }
                }
                else 
                {
                    string keylist = "";
                    foreach(string id in values)
                    {
                        if (keylist.Length == 0)
                        {
                            if (keytype == FieldType.STRING || keytype == FieldType.DATETIME || keytype == FieldType.CHAR)
                            {
                                keylist = keylist + string.Format ("'{0}'", id);
                            }
                            else
                            {
                                keylist = keylist + string.Format ("{0}", id);
                            }
                        }
                        else
                        {
                            if (keytype == FieldType.STRING || keytype == FieldType.DATETIME || keytype == FieldType.CHAR)
                            {
                                keylist = keylist + string.Format (",'{0}'", id);
                            }
                            else
                            {
                                keylist = keylist + string.Format (",{0}", id);
                            }
                        }
                    }
                    sql = string.Format("{0} IN ({1})", fieldName, keylist);
                }
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.SQLFieldSQL", string.Format ("Couldnt build field snippet for field {0}", fieldName));
                throw new ApiExpressModelException("Model.SQLFieldSQL",string.Format("Couldnt build field snippet for field {0}", fieldName));
            }
        }

        private string SQLbuilder(string SQLsection)
        {
            try 
            {
                string sql = "";  
                switch (SQLsection)
                {
                    case "BaseGetSQL":
                        if (string.IsNullOrWhiteSpace(this.CustomGetSQL))
                        {
                            sql = string.Format("select {0} from {1}", SQLbuilder("GetFields"), SQLbuilder("Table")); 
                        }
                        else
                        {
                            sql = this.CustomGetSQL;
                        }
                        break;
                    case "BaseInsertSQL":
                        if (string.IsNullOrWhiteSpace(this.CustomInsertSQL))
                        {
                            sql = string.Format("insert {0} ({1}) values ({2})", SQLbuilder("Table"), SQLbuilder("InsertFields"), SQLbuilder("InsertTemplate")); 
                        }
                        else
                        {
                            sql = this.CustomInsertSQL;
                        }
                        break;
                    case "BaseUpdateSQL":
                        if (string.IsNullOrWhiteSpace(this.CustomUpdateSQL))
                        {
                            sql = string.Format("update {0} set {1}", SQLbuilder("Table"), SQLbuilder("UpdateTemplate")); 
                        }
                        else
                        {
                            sql = this.CustomUpdateSQL;
                        }
                        break;
                    case "BaseDeleteSQL":
                        if (string.IsNullOrWhiteSpace(this.CustomDeleteSQL))
                        {
                            sql = string.Format("delete {0}", SQLbuilder("Table")); 
                        }
                        else
                        {
                            sql = this.CustomDeleteSQL;
                        }
                        break;
                    case "Table":
                        if (!string.IsNullOrWhiteSpace(this.DBSchemaName)) { sql = string.Format(" {0}.{1}", this.DBSchemaName, this.DBTableName);}
                        else sql = string.Format(" {0}", this.DBTableName);
                        break;
                    case "GetFields":
                        foreach(IModelField f in this.Fields)
                        {
                            if (!f.IsCalculated)
                            {
                                if (sql.Length > 0) 
                                {
                                    sql = sql + ",";
                                }
                                sql = sql + string.Format (" {0} as {1}", f.DBFieldName, f.Name);

                            }
                        }
                        break;
                    case "InsertFields":
                        foreach(IModelField f in this.Fields)
                        {
                            if (!f.IsCalculated && !f.IsAutoCounter)
                            {
                                if (sql.Length > 0) 
                                {
                                    sql = sql + ",";
                                }
                                sql = sql + string.Format (" {0}", f.DBFieldName);

                            }
                        }
                        break;
                    case "InsertTemplate":
                        foreach(IModelField f in this.Fields)
                        {
                            if (!f.IsCalculated && !f.IsAutoCounter)
                            {
                                if (sql.Length > 0) 
                                {
                                    sql = sql + ",";
                                }
                                string templateSQL = SQLFieldSQL(f.Name);
                                sql = sql + string.Format (" {0}", templateSQL);

                            }
                        }
                        break;
                    case "UpdateTemplate":
                        foreach(IModelField f in this.Fields)
                        {
                            if (!f.IsCalculated && !f.IsAutoCounter)
                            {
                                if (sql.Length > 0) 
                                {
                                    sql = sql + ",";
                                }
                                string templateSQL = SQLFieldSQL(f.Name);
                                sql = sql + string.Format (" {0} = {1}", f.Name, templateSQL);

                            }
                        }
                        break;
                    case "SortOrder":
                        bool useModel = false;
                        if (this.sqldata != null) 
                        { 
                            //if modeloptions 
                            if (this.sqldata.GetType() == typeof(IModelOptions))
                            {
                                if (!string.IsNullOrWhiteSpace(((IModelOptions) this.sqldata).SortOrder))
                                {
                                    useModel = true;
                                    sql = ((IModelOptions) this.sqldata).SortOrder;
                                }
                            }
                        }
                        if (useModel == false)
                        {
                            if (!string.IsNullOrWhiteSpace(this.SortOrder)) { sql = string.Format(" ORDER BY {0}", this.SortOrder);}
                            else if (this.KeyFields.Count > 0) {sql = string.Format(" ORDER BY {0}", this.KeyFields[0]);}
                                else {sql = "";}
                        }
                        break;
                    case "ScopeFilter":
                        if (!string.IsNullOrWhiteSpace(this.ScopeFilter)) { }
                        sql = string.Format(" {0}", this.ScopeFilter);
                        break;
                    case "DataFilter":
                        if (this.sqldata != null) 
                        { 
                            //if List<ids>
                            if (this.sqldata.GetType() == typeof(List<string>))
                            {
                                if (this.KeyFields.Count > 0 &&  ((List<string>) this.sqldata).Count > 0)
                                {
                                    string key = this.KeyFields[0];
                                    string fieldSQL = SQLFieldSQL(key, (List<string>) this.sqldata);
                                    sql = string.Format(" AND {0}", fieldSQL);
                                }
                            }
                            //if modeloptions 
                            // TODO model options data filter generate
                            if (this.sqldata.GetType() == typeof(IModelOptions))
                            {
                                //modeloption filtering
                                //filterchainoperator, fieldexpression
                                //  fieldexpression = [field, valueoperator, value[s]][]
                                sql = string.Format(" ");
                            }
                        }
                        break;
                    case "ObjectFilter":
                        if (this.sqldata != null) 
                        { 
                            //if List<ids>
                            if (this.sqldata is IObject)
                            {
                                if (this.KeyFields.Count > 0)
                                {
                                    foreach(string key in this.KeyFields)
                                    {
                                        string keyval = ((IObject) this.sqldata).GetValue(key);
                                        string fieldSQL = SQLFieldSQL(key, new List<string>() {keyval} );
                                        sql = sql + string.Format(" AND {0}", fieldSQL);
                                    }
                                }
                            }
                            else
                            {
                                this.Log(LogLevel.ERROR, "Model.SQLbuilder", string.Format ("Object key could not be determined for update"));
                                throw new ApiExpressModelException("Model.SQLbuilder",string.Format("Object key could not be determined for update"));
                            }
                        }
                        else
                        {
                            this.Log(LogLevel.ERROR, "Model.SQLbuilder", string.Format ("Object key not provided for update"));
                            throw new ApiExpressModelException("Model.SQLbuilder",string.Format("Object key not provided for update"));
                        }
                        break;
                    case "Paging":
                        // TODO model options data paging
                        break;
                    default:
                        this.Log(LogLevel.ERROR, "Model.SQLbuilder", string.Format ("Unknown section requested - {0}", SQLsection));
                        throw new ApiExpressModelException("Model.SQLbuilder",string.Format("Unknown section requested - {0}", SQLsection));
                }            
                return sql;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.SQLbuilder", string.Format ("Couldnt build the {0} SQL Section", SQLsection));
                throw new ApiExpressModelException("Model.SQLbuilder",string.Format("Couldnt build the {0} SQL Section", SQLsection));
            }
        }

        private FieldType GetFieldType(string fieldName)
        {
            try 
            {
                return this.Fields.First(x => x.Name == fieldName).DataType;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "Model.GetFieldType", string.Format ("Couldnt determine fieldtype of field {0}", fieldName));
                throw new ApiExpressModelException("Model.GetFieldType",string.Format("Couldnt determine fieldtype of field {0}", fieldName));
            }
        }

    }

    public class ModelField : BaseObject, IModelField
    {
        /*
        A few behaviours
        VerboseName = Name if not given
        Description = Name if not given
        DBFieldName = Name if not given

        defaults
        Datatype = string
        MaxLength = 0
        IsPrimaryKey = false
        IsUnique = false
        IsRequired = true
        IsAutoCounter = false 
         */

        private string _verboseName;
        private string _description;
        private string _DBfieldname;

        public string Name {get; set;}
        public string VerboseName 
        {
            get { if (string.IsNullOrWhiteSpace(_verboseName)) { return this.Name;} else { return this._verboseName; } }
            set { this._verboseName = value; }
        }
        public string Description 
        {
            get { if (string.IsNullOrWhiteSpace(_description)) { return this.Name;} else { return this._description; } }
            set { this._description = value; }
        }
        public int DisplayIndex {get; set;}
        public string DBFieldName 
        {
            get { if (string.IsNullOrWhiteSpace(_DBfieldname)) { return this.Name;} else { return this._DBfieldname; } }
            set { this._DBfieldname = value; }
        }
        public FieldType DataType {get; set;}
        public Boolean IsPrimaryKey {get; set;}
        public Boolean IsUnique {get; set;}
        public Boolean IsAutoCounter {get; set;}
        public Boolean IsRequired {get; set;}
        public string DefaultValue {get; set;}
        public float MinLength {get; set;}
        public float MaxLength {get; set;}
        public Boolean IsCalculated
        {
            get { if(!string.IsNullOrWhiteSpace(this.ValueFormula)){ return true;} else {return false;}}
        }
        public string ValueFormula {get; set;}
        public string RuleFormula {get; set;}
 
        
        public ModelField() : base()
        {
            this.DataType = FieldType.STRING;
            this.MaxLength = 0;
            this.IsPrimaryKey = false;
            this.IsUnique = false;
            this.IsRequired = true;
            this.IsAutoCounter = false;
            this.MinLength = 0;
            this.MaxLength = 0;
        }
        public ModelField(string Name) : this()
        {
            this.Name = Name;
        }
        public ModelField(string Name, FieldType DataType, float MaxLength, Boolean IsRequired ) : this()
        {
            this.Name = Name;
            this.DataType = DataType;
            this.MaxLength = MaxLength;
            this.IsRequired = IsRequired;
        }
    }

    public class ModelFieldMapping : BaseObject, IModelFieldMapping
    {

        public string PropertyName {get; set;}
        public string FieldName {get; set;}
        public FieldType FieldType {get; set;}
        public Boolean IsRequired {get; set;}
        public ModelFieldMapping() 
        {
        }
        public ModelFieldMapping(string Name) : this()
        {
            this.PropertyName = Name;
            this.FieldName = Name;
        }
        public ModelFieldMapping(string PropName, string FieldName, FieldType FieldType = FieldType.STRING, Boolean IsRequired = true ) : this()
        {
            this.PropertyName = PropName;
            this.FieldName = FieldName;
            this.FieldType = FieldType;
            this.IsRequired = IsRequired;
        }
    }

    public class ModelRelationship : BaseObject
    {

    }

    public class ModelRule : BaseObject, IModelRule
    {
        public string Name {get; set;}
        public string FieldName {get; set;}
        public string Description {get; set;}
        public RuleType RuleType {get; set;}

        /*Rule Types
            standard (from field metadata)
                datatype
                IsRequired
                length
                unique
            regex (of value)
            function (taking value, obj)
        
         */
        public string RuleValue {get; set;}


        public ModelRule() : base()
        {
        }
        public ModelRule(string Name,  RuleType RuleType, string RuleValue, string FieldName) : this()
        {
            this.Name = Name;
            this.RuleType = RuleType;
            this.RuleValue = RuleValue;
            this.FieldName = FieldName;
        }
        
    }
}
