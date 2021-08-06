using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

using System.IO;
using CloudAPI.Utils;

namespace CloudAPI.Runtime
{
    public class ConfigService : BaseService, IConfigService
    {

        public ConfigProvider ConfigProvider {get; set;}
        public string ConfigProviderDetail {get; set;}

        public ConfigService() 
        {
            this.ServiceType = ServiceType.CONFIGURATION;
        }
        public string GetConfig(string section, string key)
        {
            return ((IConfigurationSection) this.GetConfigSection(section))[key];
        }
        public object GetConfigSection(string section)
        {
             var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)      
            .AddJsonFile($"appsettings.{this.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            return configuration.GetSection(section);
        }
        

    }
}