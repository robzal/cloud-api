using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public interface IDataService
    {

        IDbCommand Command {get; set;}
        IDataReader Reader {get; set;}
        IDbConnection Connection {get; set;}
        DataProvider Provider {get; set;}
        string ConnectionString {get; set;}
        void OpenConnection();
        void OpenConnection(DataProvider provider, string ConnectionString);
        void CloseConnection();
        Task Execute(string SQL);
        Task<IDataReader> ExecuteReader(string SQL);
        Task<JSObject> Get<T> (IModel model, string ID);
        Task<JSObject> GetList<T> (IModel model);
        Task<JSObject> GetList<T> (IModel model, List<string> IDs);
        Task<JSObject> GetList<T> (IModel model, IModelOptions queryoptions);
        Task<IServiceResult> Insert<T> (IModel model, IObject obj);
        Task<IServiceResult> Update<T> (IModel model, IObject obj);
        Task<IServiceResult> Delete<T> (IModel model, string ID);
        Task<IServiceResult> DeleteList<T> (IModel model);
        Task<IServiceResult> DeleteList<T> (IModel model, List<string> IDs);

    }
}
