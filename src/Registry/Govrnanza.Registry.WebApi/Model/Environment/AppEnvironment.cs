using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Model.Environment
{
    public class ConfigItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AppEnvironment
    {
        public string EnvironmentName { get; set; }
        public string HostName { get; set; }
        public List<ConfigItem> Configuration { get; set; }
    }
}
