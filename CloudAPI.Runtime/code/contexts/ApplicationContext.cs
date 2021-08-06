﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAPI.Runtime
{
    public class ApplicationContext : BaseObject, IApplicationContext
    {
        public IPublisher Publisher {get; set;}
        public IApplication Application {get; set;}
        public ITenant Tenant {get; set;}
    
        public List<IModel> Models { get; set; }
        public List<IService> Services { get; set; }
        public List<ISetting> Settings { get; set; }
        public List<ILibrary> Libraries {get; set;}

        public ApplicationContext() 
        {
            this.Models = new List<IModel>();
            this.Services = new List<IService>();
            this.Settings = new List<ISetting>();
            this.Libraries = new List<ILibrary>();            
        }
    }
}
