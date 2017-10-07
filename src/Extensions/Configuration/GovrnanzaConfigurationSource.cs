using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Govrnanza.Extensions.Configuration
{
    public class GovrnanzaConfigurationSource : IConfigurationSource
    {
        private SecretsMode _secretsMode;
        private List<string> _pathEnvironmentVariables;

        public GovrnanzaConfigurationSource(SecretsMode secretsMode,
            List<string> pathEnvironmentVariables)
        {
            _secretsMode = secretsMode;
            _pathEnvironmentVariables = pathEnvironmentVariables;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new GovrnanzaConfigurationProvider(
                _secretsMode,
                _pathEnvironmentVariables);
        }
    }
}
