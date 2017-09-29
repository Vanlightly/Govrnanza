using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Secrets
{
    public class SecretsResolver
    {
        public static string ResolveEmbeddedSecret(string environmentName, IConfiguration configuration, string inputText)
        {
            var secretsResults = Regex.Matches(inputText, @"(?<secret>\{secret:[\s\w-\.]+\})");

            foreach (Match secretTagResult in secretsResults)
            {
                var groupText = secretTagResult.Groups[0].Value;
                var secretId = groupText.Substring(8, groupText.Length - 9).Trim();

                string storedSecret = string.Empty;
                if (environmentName.Equals("Development"))
                {
                    storedSecret = configuration[$"Secret_{secretId}"];
                }
                else
                {
                    storedSecret = LoadSecret(secretId);
                }
                
                inputText = inputText.Replace(groupText, storedSecret);
            }

            return inputText;
        }

        private static string LoadSecret(string secretId)
        {
            var pathToSecret = Environment.GetEnvironmentVariable(secretId + "_SECRET_FILE");

            if (!File.Exists(pathToSecret))
            {
                throw new Exception($"SecretId: {secretId} does not exist at " + pathToSecret + " or this service does not have access to it");
            }

            var storedSecret = File.ReadAllText(pathToSecret);

            return storedSecret;
        }
    }
}
