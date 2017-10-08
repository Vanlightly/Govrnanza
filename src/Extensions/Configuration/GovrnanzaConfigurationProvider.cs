using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Govrnanza.Extensions.Configuration
{
    public class GovrnanzaConfigurationProvider : ConfigurationProvider
    {
        private SecretsMode _secretsMode;
        private List<string> _pathEnvironmentVariables;

        public GovrnanzaConfigurationProvider(SecretsMode secretsMode,
            List<string> pathEnvironmentVariables)
        {
            _secretsMode = secretsMode;
            _pathEnvironmentVariables = pathEnvironmentVariables;
        }

        public override void Load()
        {
            try
            {
                foreach (var pathEnvVariable in _pathEnvironmentVariables)
                {
                    var configPath = Environment.GetEnvironmentVariable(pathEnvVariable);
                    if (configPath != null)
                    {
                        var jsonBytes = LoadJsonFile(configPath);

                        var secretsResolver = new SecretsResolver(_secretsMode);
                        var parser = new JsonConfigurationFileParser(secretsResolver);
                        var configDict = parser.Parse(new MemoryStream(jsonBytes));

                        foreach (var key in configDict.Keys)
                            Data.Add(key, configDict[key]);
                    }
                }
            }
            catch(ConfigurationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConfigurationException("Failed on loading json file", ex);
            }
        }

        private byte[] LoadJsonFile(string path)
        {
            var files = Directory.GetFiles(".");
            if (!File.Exists(path))
                throw new ConfigurationException($"The file at path: {path} does not exist");

            return File.ReadAllBytes(path);
        }
    }
}
