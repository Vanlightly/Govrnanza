//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Govrnanza.Registry.WebApi.Configuration.Secrets
//{
//    public class SecretsResolver
//    {
//        private SecretsMode _secretsMode;
//        //private Dictionary<string, string> _insecureSecrets;

//        public SecretsResolver(SecretsMode secretsMode)
//        {
//            _secretsMode = secretsMode;
//        }

//        public string ResolveEmbeddedSecret(string inputText)
//        {
//            var secretsResults = Regex.Matches(inputText, @"(?<secret>\{secret:[\s\w-\.]+\})");

//            foreach (Match secretTagResult in secretsResults)
//            {
//                var groupText = secretTagResult.Groups[0].Value;
//                var secretId = groupText.Substring(8, groupText.Length - 9).Trim();

//                string storedSecret = LoadSecret(secretId);
//                //if (_secretsMode == SecretsMode.Secure)
//                //{
//                //    storedSecret = LoadSecureSecret(secretId);
//                //}
//                //else
//                //{
//                //    if (_insecureSecrets.ContainsKey(secretId))
//                //        storedSecret = _insecureSecrets[secretId];
//                //    else
//                //        storedSecret = string.Empty;
//                //}

//                inputText = inputText.Replace(groupText, storedSecret);
//            }

//            return inputText;
//        }

//        private string LoadSecret(string secretId)
//        {
//            var pathToSecret = Environment.GetEnvironmentVariable(secretId + "_SECRET_FILE");
//            if(_secretsMode == SecretsMode.Enforce)
//            {
//                if (!pathToSecret.StartsWith("/run/secrets/", StringComparison.OrdinalIgnoreCase))
//                    throw new ConfigurationException("Cannot load secrets from local files when run in Enforce mode");
//            }

//            if (!File.Exists(pathToSecret))
//            {
//                throw new ConfigurationException($"SecretId: {secretId} does not exist at " + pathToSecret + " or this service does not have access to it");
//            }

//            var storedSecret = File.ReadAllText(pathToSecret);

//            return storedSecret;
//        }

//        //private void LoadInsecureSecrets()
//        //{
//        //    _insecureSecrets = new Dictionary<string, string>();

//        //    if (Directory.Exists("InsecureSecretFiles"))
//        //    {
//        //        var files = Directory.GetFiles("InsecureSecretFiles");
//        //        foreach (var file in files)
//        //        {
//        //            var secretId = Path.GetFileNameWithoutExtension(file);
//        //            var secret = File.ReadAllText(file);

//        //            _insecureSecrets.Add(secretId, secret);
//        //        }
//        //    }
//        //}
//    }
//}
