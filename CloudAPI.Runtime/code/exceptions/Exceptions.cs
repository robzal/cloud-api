using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class ApiExpressException: Exception
    { 
        public ApiExpressException () :this("", "") {}
        public ApiExpressException (string message) : this("", message) {}
        public ApiExpressException (string source, string message) : base(message) { base.Source = source; }
    }

    public class ApiExpressDataException: ApiExpressException
    {
        public ApiExpressDataException () :this("", "") {}
        public ApiExpressDataException (string message) : this("", message) {}
        public ApiExpressDataException (string source, string message) : base(message) { base.Source = source; }
    }
    
    public class ApiExpressModelException: ApiExpressException
    {
        public ApiExpressModelException () :this("", "") {}
        public ApiExpressModelException (string message) : this("", message) {}
        public ApiExpressModelException (string source, string message) : base(message) { base.Source = source; }
    }
    
    public class ApiExpressSecurityException: ApiExpressException
    {
        public ApiExpressSecurityException () :this("", "") {}
        public ApiExpressSecurityException (string message) : this("", message) {}
        public ApiExpressSecurityException (string source, string message) : base(message) { base.Source = source; }
    }
    
    public class ApiExpressRequestException: ApiExpressException
    {
        public ApiExpressRequestException () :this("", "") {}
        public ApiExpressRequestException (string message) : this("", message) {}
        public ApiExpressRequestException (string source, string message) : base(message) { base.Source = source; }
    }

    public class ApiExpressServiceException: ApiExpressException
    {
        public ApiExpressServiceException () :this("", "") {}
        public ApiExpressServiceException (string message) : this("", message) {}
        public ApiExpressServiceException (string source, string message) : base(message) { base.Source = source; }
    }
    
}
