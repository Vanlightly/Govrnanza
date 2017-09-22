using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Docs
{
    public class ApiVersionDescriptions
    {
        private Dictionary<string, string> _descriptions;

        public ApiVersionDescriptions()
        {
            _descriptions = new Dictionary<string, string>();
        }

        public void AddDescription(string version, string description)
        {
            if (!_descriptions.ContainsKey(version))
                _descriptions.Add(version, description);
        }

        public string GetDescription(string version)
        {
            if(_descriptions.ContainsKey(version))
                return _descriptions[version];

            return string.Empty;
        }

    }
}
