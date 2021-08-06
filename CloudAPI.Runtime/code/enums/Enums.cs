using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public enum Scope
    {
        SYSTEM = 0,
        PUBLISHER = 1,
        APPLICATION = 2,
        TENANT = 3,
        USERGROUP = 4,
        USER = 5
    }

    public enum FieldType
    {
        STRING,
        CHAR,
        BOOLEAN,
        BYTE,
        SHORT,
        INT,
        LONG,
        FLOAT,
        DOUBLE,
        DECIMAL,
        DATETIME,
        OBJECT,
        UNKNOWN

    }

    public enum ServiceType
    {
        GENERIC,
        LOG,
        AUTHENTICATION,
        DBSTORAGE,
        FILESTORAGE,
        DBCACHE,
        FILECACHE,
        MESSAGING,
        QUEUING,
        CONFIGURATION
    }

    public enum ServiceStatus
    {
        RUNNING,
        STOPPED,
        ERRORED,
        UNKNOWN
    }

    public enum SettingType
    {
        APPLICATION = 0,
        MODEL = 1,
        SERVICE =2 ,
        LIBRARY = 3
    }
    
    public enum LoginType 
    {
        APIEXPRESS = 0,
        LOCALDB = 1,
        LDAP = 2,
        MICROSOFT = 3,
        GOOGLE = 4,
        FACEBOOK = 5
    }

    public enum DataProvider 
    {
        GENERIC,
        MYSQL,
        MSSQL,
        SQLITE,
        POSTGRES
    }

    public enum LogLevel
    {
        DEBUG = 10,
        INFO = 20,
        WARNING = 30,
        ERROR = 40,
        NONE = 50
    }
    public enum LogProvider
    {
        NLOG,
        CLOUDWATCH,
        CUSTOM,
        NONE
    }

    public enum ConfigProvider
    {
        JSONFILE,
        NONE
    }

    public enum RuleType
    {
        FIELDTYPE = 10,
        FIELDREGEX = 20,
        DATAUNIQUE = 15,
        RECORDFUNCTION = 25,
        DATAFUNCTION = 30
    }
}
