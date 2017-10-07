//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Govrnanza.Registry.WebApi.Configuration
//{
//    public enum SecretsMode
//    {
//        NoCheck,
//        Enforce
//    }

//    public static class ConfigurationBuilderExtensions
//    {
//        public static IConfigurationBuilder AddConfig(this IConfigurationBuilder builder,
//            IHostingEnvironment hostingEnvironment)
//        {
//            var secretsMode = SecretsMode.Enforce;
//            if (!hostingEnvironment.IsProduction())
//                secretsMode = SecretsMode.NoCheck;

//            var settingsConfigSource = new RegistryConfigurationSource(secretsMode);
//            return builder.Add(settingsConfigSource);
//        }
//    }
//}
