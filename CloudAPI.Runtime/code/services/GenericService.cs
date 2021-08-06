using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class GenericService : BaseService
    {
        public GenericService()
        {
            this.ServiceType = ServiceType.GENERIC;
        }
    }
}
