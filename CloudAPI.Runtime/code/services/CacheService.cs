using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public class CacheService : BaseService, ICacheService
    {
        public CacheService()
        {
            this.ServiceType = ServiceType.DBCACHE;
        }
    }
}