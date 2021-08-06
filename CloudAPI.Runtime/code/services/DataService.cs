using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Data.Common;
using CloudAPI.Utils;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace CloudAPI.Runtime
{
    public class DataService : BaseService, IDataService
    {
        private int RowsAffected;
        public IDbCommand Command {get; set;}
        public IDataReader Reader {get; set;}
        public IDbConnection Connection {get; set;}
        public DataProvider Provider {get; set;}
        public string ConnectionString {get; set;}
        private string SQLcommand {get; set;}

        public DataService() : base()
        {
            this.ServiceType = ServiceType.DBSTORAGE;
            this.Result = new DataResult();
        }
        private void GetConnection()
        {
            try
            {
                // TODO Postgres and ODBC providers
                //create the appropriate conn object if not already
                if (this.Connection == null)
                {
                    switch (this.Provider)
                    {
                        case DataProvider.MYSQL:
                            this.Connection = new MySqlConnection(this.ConnectionString);
                            break;
                        case DataProvider.MSSQL:
                            this.Connection = new SqlConnection(this.ConnectionString);
                            break;
                        case DataProvider.SQLITE:
                            this.Connection = new SqliteConnection(this.ConnectionString);
                            break;
                        default:
                            this.Log(LogLevel.ERROR, "DataService.GetConnection", string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                            throw new ApiExpressServiceException("DataService.GetConnection",string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                    }                        
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetConnection", string.Format ("Couldnt create a data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
                throw new ApiExpressServiceException("DataService.GetConnection",string.Format ("Couldnt create a data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
            }
        }
        public void OpenConnection ()
        {
            try
            {
                this.GetConnection();
                if (this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.Open();
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.OpenConnection", string.Format ("Couldnt open data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
                throw new ApiExpressServiceException("DataService.OpenConnection",string.Format ("Couldnt open data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
            }
        }
        public void OpenConnection (DataProvider provider, string connectionString)
        {
            try
            {
                this.ConnectionString = connectionString;
                this.Provider = provider;
                this.OpenConnection();
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.OpenConnection", string.Format ("Couldnt open data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
                throw new ApiExpressServiceException("DataService.OpenConnection",string.Format ("Couldnt open data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
            }
        }
        public void CloseConnection () 
        {
            try
            {
                if (this.Connection.State != ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.CloseConnection", string.Format ("Couldnt close data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
                throw new ApiExpressServiceException("DataService.CloseConnection",string.Format ("Couldnt close data connection for the {0} provider to {1}", this.Provider.ToString(), this.ConnectionString));
            }
        }
        private void GetCommand(string SQL)
        {
            try
            {
                this.SQLcommand = SQL;
                //create the appropriate conn object if not already
                if (this.Command == null)
                {
                    switch (this.Provider)
                    {
                        case DataProvider.MYSQL:
                            this.Command = new MySqlCommand(this.SQLcommand, (MySqlConnection) this.Connection);
                            break;
                        case DataProvider.MSSQL:
                            this.Command = new SqlCommand(this.SQLcommand, (SqlConnection) this.Connection);
                            break;
                        case DataProvider.SQLITE:
                            this.Command = new SqliteCommand(this.SQLcommand, (SqliteConnection) this.Connection);
                            break;
                        default:
                            this.Log(LogLevel.ERROR, "DataService.GetCommand", string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                            throw new ApiExpressServiceException("DataService.GetCommand",string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                    }
                        
                }
                else
                {
                    this.Command.CommandText = SQL;
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetCommand", string.Format ("Couldnt create command for the {0} provider for command {1}", this.Provider.ToString(), SQL));
                throw new ApiExpressServiceException("DataService.GetCommand",string.Format ("Couldnt create command for the {0} provider for command {1}", this.Provider.ToString(), SQL));
            }

        }
        public async Task<IDataReader> ExecuteReader(string SQL) 
        {
            try {
                GetConnection();
                OpenConnection();
                GetCommand(SQL);
                // async execute, so we'll need cast back to concrete implementations
                try {this.Reader.Close();} catch {}
                switch (this.Provider)
                {
                    case DataProvider.MYSQL:
                        this.Reader = await ((MySqlCommand) this.Command).ExecuteReaderAsync();
                        break;
                    case DataProvider.MSSQL:
                        this.Reader = await ((SqlCommand) this.Command).ExecuteReaderAsync();
                        break;
                    case DataProvider.SQLITE:
                        this.Reader = await ((SqliteCommand) this.Command).ExecuteReaderAsync();
                        break;
                    default:
                        this.Log(LogLevel.ERROR, "DataService.ExecuteReader", string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                        throw new ApiExpressServiceException("DataService.ExecuteReader",string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                }                
                this.Result.ResultCode = 0;
                this.Result.Message = "Success";
                this.Result.RowsAffected = this.Reader.RecordsAffected;
                return this.Reader;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.ExecuteReader", string.Format ("Couldnt create DBreader for the {0} provider for command {1}", this.Provider.ToString(), SQL));
                throw new ApiExpressServiceException("DataService.ExecuteReader",string.Format ("Couldnt create DBreader for the {0} provider for command {1}", this.Provider.ToString(), SQL));
            }
        }
        public async Task Execute(string SQL)
        {
            try {
                GetConnection();
                OpenConnection();
                GetCommand(SQL);
                // async execute, so we'll need cast back to concrete implementations
                switch (this.Provider)
                {
                    case DataProvider.MYSQL:
                        this.RowsAffected = await ((MySqlCommand) this.Command).ExecuteNonQueryAsync();
                        break;
                    case DataProvider.MSSQL:
                        this.RowsAffected = await ((SqlCommand) this.Command).ExecuteNonQueryAsync();
                        break;
                    case DataProvider.SQLITE:
                        this.RowsAffected = await ((SqliteCommand) this.Command).ExecuteNonQueryAsync();
                        break;
                    default:
                        this.Log(LogLevel.ERROR, "DataService.Execute", string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                        throw new ApiExpressServiceException("DataService.Execute",string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                }                
                this.Result.ResultCode = 0;
                this.Result.Message = "Success";
                this.Result.RowsAffected = this.RowsAffected;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Execute", string.Format ("Couldnt execute SQL command for the {0} provider for command {1}", this.Provider.ToString(), SQL));
                throw new ApiExpressServiceException("DataService.Execute",string.Format ("Couldnt execute SQL command for the {0} provider for command {1}", this.Provider.ToString(), SQL));
            }
        }
        public string ExecuteScalar(string SQL)
        {
            try {
                GetConnection();
                OpenConnection();
                GetCommand(SQL);
                string id = (string) this.Command.ExecuteScalar();
                this.Result.ResultCode = 0;
                this.Result.Message = "Success";
                this.Result.RowsAffected = 1;
                return id;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.ExecuteScalar", string.Format ("Couldnt execute SQL command for the {0} provider for command {1}", this.Provider.ToString(), SQL));
                throw new ApiExpressServiceException("DataService.ExecuteScalar",string.Format ("Couldnt execute SQL command for the {0} provider for command {1}", this.Provider.ToString(), SQL));
            }
        }
        private IObject GetObject<T>(IDataReader reader, List<IModelFieldMapping> mapping)
        {
            try 
            {
                IObject obj = (IObject) Activator.CreateInstance(typeof(T));
                foreach(IModelFieldMapping map in mapping) 
                {
                    obj.SetValue(map.PropertyName, reader[map.FieldName]);
                }
                return obj;
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetObject<T>", string.Format ("Couldnt create an object of type {0} from DBReader", typeof(T).FullName));
                throw new ApiExpressServiceException("DataService.GetObject<T>",string.Format ("Couldnt create an object of type {0} from DBReader", typeof(T).FullName));
            }
        }
        private List<string> GetInsertedIDs() 
        {
            try
            {
                string id = "";
                switch (this.Provider)
                {
                    case DataProvider.MYSQL:
                        id = ExecuteScalar("SELECT scope_identity()");
                        break;
                    case DataProvider.MSSQL:
                        id = ExecuteScalar("SELECT LAST_INSERT_ID()");
                        break;
                    case DataProvider.SQLITE:
                        id = ExecuteScalar("SELECT last_insert_rowid()");
                        break;
                    default:
                        this.Log(LogLevel.ERROR, "DataService.GetInsertedIDs", string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                        throw new ApiExpressServiceException("DataService.GetInsertedIDs",string.Format ("Data Provider Not Supported - {0}", this.Provider.ToString()));
                }
                if (id == "")
                {
                    this.Log(LogLevel.ERROR, "DataService.GetInsertedIDs", string.Format ("Couldnt determine the ID of the records added"));
                    throw new ApiExpressServiceException("DataService.GetInsertedIDs",string.Format ("Couldnt determine the ID of the records added"));
                }
                else
                {
                    return new List<string>() {id};
                }
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetInsertedIDs", string.Format ("Couldnt determine the ID of the records added"));
                throw new ApiExpressServiceException("DataService.GetInsertedIDs",string.Format ("Couldnt determine the ID of the records added"));
            }
        }
        public async Task<JSObject> Get<T> (IModel model, string ID) 
        {
            try
            {
                string sql = model.GetSQL(new List<string>() {ID});
                IDataReader reader = await this.ExecuteReader(sql);
                JSObject o = new JSObject();

                //only looking for the first record in this Fetch.
                if (reader.Read())
                {
                    IObject obj = GetObject<T>(reader, model.Mappings);
                    o.Serialize<T>(obj);
                }
                return o;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Get<T>", string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL(new List<string>() {ID})));
                throw new ApiExpressServiceException("DataService.Get<T>",string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL(new List<string>() {ID})));
            }
        }
        public async Task<JSObject> GetList<T> (IModel model) 
        {
            try
            {
                IDataReader reader = await this.ExecuteReader(model.GetSQL());
                JSObject o = new JSObject();
                List<IObject> list = new List<IObject>();

                while (reader.Read())
                {
                    IObject obj = GetObject<T>(reader, model.Mappings);
                    list.Add(obj);
                }
                o.SerializeList<T>(list.Cast<IObject>().ToList());
                return o;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetList<T>", string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL()));
                throw new ApiExpressServiceException("DataService.GetList<T>",string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL()));
            }
        }
        public async Task<JSObject> GetList<T> (IModel model, List<string> IDs) 
        {
            try
            {
                IDataReader reader = await this.ExecuteReader(model.GetSQL(IDs));
                JSObject o = new JSObject();
                List<IObject> list = new List<IObject>();

                while (reader.Read())
                {
                    IObject obj = GetObject<T>(reader, model.Mappings);
                    list.Add(obj);
                }
                o.SerializeList<T>(list.Cast<IObject>().ToList());
                return o;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetList<T>", string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL(IDs)));
                throw new ApiExpressServiceException("DataService.GetList<T>",string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL(IDs)));
            }
        }
        public async Task<JSObject> GetList<T> (IModel model, IModelOptions queryoptions) 
        {
            try
            {
                IDataReader reader = await this.ExecuteReader(model.GetSQL(queryoptions));
                JSObject o = new JSObject();
                List<IObject> list = new List<IObject>();

                while (reader.Read())
                {
                    IObject obj = GetObject<T>(reader, model.Mappings);
                    list.Add(obj);
                }
                o.SerializeList<T>(list.Cast<IObject>().ToList());
                return o;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.GetList<T>", string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL(queryoptions)));
                throw new ApiExpressServiceException("DataService.GetList<T>",string.Format ("Couldnt create a JSObject of type {0} from DBReader using {1}", typeof(T).FullName, model.GetSQL(queryoptions)));
            }
        }
        public async Task<IServiceResult> Insert<T> (IModel model, IObject obj) 
        {
            JSObject o = new JSObject();
            string insertSQL = model.InsertSQL(obj);
            await Execute(insertSQL);
            string counter = "";
            string PK = "";
            try
            {
                // check autocounter fields, otherwise return PK[0] value
                foreach (IModelField f in model.Fields)
                {
                    //find counter or key fields
                    if (f.IsAutoCounter)
                    {
                        counter = f.Name;
                        break;
                    }
                    if (f.IsPrimaryKey)
                    {
                        PK = f.Name;
                    }
                }
                if (counter != "")
                {
                    this.Result.ObjectIDs = GetInsertedIDs();
                }
                else if (PK != "")
                {
                    string id = obj.GetValue(PK);
                    this.Result.ObjectIDs = new List<string>() {id};                    
                }
                else 
                {
                    this.Log(LogLevel.ERROR, "DataService.Insert<T>", string.Format ("Couldnt insert a new object of type {0} using {1}", typeof(T).FullName, model.InsertSQL(obj)));
                    throw new ApiExpressServiceException("DataService.Insert<T>",string.Format ("Couldnt insert a new object of type {0} using {1}", typeof(T).FullName, model.InsertSQL(obj)));
                }
                return this.Result;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Insert<T>", string.Format ("Couldnt insert a new object of type {0} using {1}", typeof(T).FullName, model.InsertSQL(obj)));
                throw new ApiExpressServiceException("DataService.Insert<T>",string.Format ("Couldnt insert a new object of type {0} using {1}", typeof(T).FullName, model.InsertSQL(obj)));
            }
        }
        public async Task<IServiceResult> Update<T> (IModel model, IObject obj) 
        {
            JSObject o = new JSObject();
            string updateSQL = model.UpdateSQL(obj);
            await Execute(updateSQL);
            string PK = "";
            try
            {
                // check autocounter fields, otherwise return PK[0] value
                foreach (IModelField f in model.Fields)
                {
                    //find counter or key fields
                    if (f.IsPrimaryKey)
                    {
                        PK = f.Name;
                    }
                }
                if (PK != "")
                {
                    string id = obj.GetValue(PK);
                    this.Result.ObjectIDs = new List<string>() {id};                    
                }
                else 
                {
                    this.Log(LogLevel.ERROR, "DataService.Update<T>", string.Format ("Couldnt update object of type {0} using {1}", typeof(T).FullName, model.UpdateSQL(obj)));
                    throw new ApiExpressServiceException("DataService.Update<T>",string.Format ("Couldnt update object of type {0} using {1}", typeof(T).FullName, model.UpdateSQL(obj)));
                }
                return this.Result;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Update<T>", string.Format ("Couldnt update object of type {0} using {1}", typeof(T).FullName, model.UpdateSQL(obj)));
                throw new ApiExpressServiceException("DataService.Update<T>",string.Format ("Couldnt update object of type {0} using {1}", typeof(T).FullName, model.UpdateSQL(obj)));
            }
        }
        public async Task<IServiceResult> Delete<T> (IModel model, string ID) 
        {
            try
            {
            string deleteSQL = model.DeleteSQL(new List<string>(){ID});
            await Execute(deleteSQL);
            return this.Result;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Delete<T>", string.Format ("Couldnt delete objects of type {0} using {1}", typeof(T).FullName, model.DeleteSQL(new List<string>(){ID})));
                throw new ApiExpressServiceException("DataService.Delete<T>",string.Format ("Couldnt delete objects of type {0} using {1}", typeof(T).FullName, model.DeleteSQL(new List<string>(){ID})));
            }
        }
        public async Task<IServiceResult> DeleteList<T> (IModel model) 
        {
            try
            {
                string deleteSQL = model.DeleteSQL();
                await Execute(deleteSQL);
                return this.Result;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Delete<T>", string.Format ("Couldnt delete objects of type {0} using {1}", typeof(T).FullName, model.DeleteSQL()));
                throw new ApiExpressServiceException("DataService.Delete<T>",string.Format ("Couldnt delete objects of type {0} using {1}", typeof(T).FullName, model.DeleteSQL()));
            }
        }
        public async Task<IServiceResult> DeleteList<T> (IModel model, List<string> IDs) 
        {
            try
            {
                string deleteSQL = model.DeleteSQL(IDs);
                await Execute(deleteSQL);
                return this.Result;        
            }
            catch (ApiExpressException awex)
            {
                throw awex;
            }
            catch (Exception ex)
            {
                this.Log(LogLevel.ERROR, "DataService.Delete<T>", string.Format ("Couldnt delete objects of type {0} using {1}", typeof(T).FullName, model.DeleteSQL(IDs)));
                throw new ApiExpressServiceException("DataService.Delete<T>",string.Format ("Couldnt delete objects of type {0} using {1}", typeof(T).FullName, model.DeleteSQL(IDs)));
            }
        }
    }

    public class DataResult : IServiceResult 
    {
        public int ResultCode {get; set;}
        public string Message {get; set;}
        public int RowsAffected {get; set;}
        public List<string> ObjectIDs {get; set;}

        public DataResult()
        {
            this.ObjectIDs = new List<string>();
        } 
    }
}