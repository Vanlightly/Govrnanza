//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Govrnanza.Registry.WebApi.Configuration
//{
//    public class RegistryConfigurationSource : IConfigurationSource
//    {
//        private SecretsMode _secretsMode;
                
//        public RegistryConfigurationSource(SecretsMode secretsMode)
//        {
//            _secretsMode = secretsMode;
//        }

//        public IConfigurationProvider Build(IConfigurationBuilder builder)
//        {
//            return new RegistryConfigurationProvider(
//                _secretsMode,
//                this);
//        }
//    }
//}
