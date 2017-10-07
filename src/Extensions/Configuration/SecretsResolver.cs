using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Govrnanza.Extensions.Configuration
{
    internal class SecretsResolver
    {
        private SecretsMode _secretsMode;
        
        public SecretsResolver(SecretsMode secretsMode)
        {
            _secretsMode = secretsMode;
        }

        public string ResolveEmbeddedSecret(string inputText)
        {
            var secretsResults = Regex.Matches(inputText, @"(?<secret>\{secret:[\s\w-\.]+\})");

            foreach (Match secretTagResult in secretsResults)
            {
                var groupText = secretTagResult.Groups[0].Value;
                var secretId = groupText.Substring(8, groupText.Length - 9).Trim();

                string storedSecret = LoadSecret(secretId);
                inputText = inputText.Replace(groupText, storedSecret);
            }

            return inputText;
        }

        private string LoadSecret(string secretId)
        {
            var pathToSecret = Environment.GetEnvironmentVariable(secretId + "_SECRET_FILE");
            if(_secretsMode == SecretsMode.DockerSecrets)
            {
                if (!pathToSecret.StartsWith("/run/secrets/", StringComparison.OrdinalIgnoreCase))
                    throw new ConfigurationException("Cannot load secrets from local files when run in Enforce mode");
            }

            if (!File.Exists(pathToSecret))
            {
                throw new ConfigurationException($"SecretId: {secretId} does not exist at " + pathToSecret + " or this service does not have access to it");
            }

            var storedSecret = File.ReadAllText(pathToSecret);

            return storedSecret;
        }
    }
}
