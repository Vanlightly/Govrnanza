using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Extensions.Configuration
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {

        }

        public ConfigurationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
