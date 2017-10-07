using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Govrnanza.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddGovrnanzaConfig(this IConfigurationBuilder builder, SecretsMode secretsMode, params string[] pathEnvironmentVariables)
        {
            var settingsConfigSource = new GovrnanzaConfigurationSource(secretsMode, pathEnvironmentVariables.ToList());
            return builder.Add(settingsConfigSource);
        }
    }
}
