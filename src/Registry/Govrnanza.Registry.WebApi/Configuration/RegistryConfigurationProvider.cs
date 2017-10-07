//using Govrnanza.Registry.WebApi.Configuration.Secrets;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.IO;

//namespace Govrnanza.Registry.WebApi.Configuration
//{
//    public class RegistryConfigurationProvider : ConfigurationProvider
//    {
//        private string _configEnvironmentVariable;
//        private SecretsMode _secretsMode;
//        private readonly RegistryConfigurationSource _source;

//        public RegistryConfigurationProvider(SecretsMode secretsMode,
//            RegistryConfigurationSource source)
//        {
//            _secretsMode = secretsMode;
//            _configEnvironmentVariable = "CONFIGURATION_FILE";
//            _source = source;
//        }

//        public override void Load()
//        {
//            try
//            {
//                var configPath = Environment.GetEnvironmentVariable(_configEnvironmentVariable);
//                var jsonBytes = LoadJsonFile(configPath);

//                var secretsResolver = new SecretsResolver(_secretsMode);
//                var parser = new JsonConfigurationFileParser(secretsResolver);

//                Data = parser.Parse(new MemoryStream(jsonBytes));
//            }
//            catch(ConfigurationException cex)
//            {
//                throw;
//            }
//            catch (Exception ex)
//            {
//                throw new ConfigurationException("Failed on loading json file", ex);
//            }
//        }

//        private byte[] LoadJsonFile(string path)
//        {
//            if (!File.Exists(path))
//                throw new ConfigurationException($"The file at path: {path} does not exist");

//            return File.ReadAllBytes(path);
//        }

//        // for later use if I need a reloadable config
//        //public IChangeToken Watch()
//        //{
//        //    Task.Run(() =>
//        //    {
//        //        while (true)
//        //        {
//        //            Thread.Sleep(_checkForChangesInterval);

//        //            try
//        //            {
//        //                var configPath = Environment.GetEnvironmentVariable(_configEnvironmentVariable);
//        //                var jsonBytes = LoadJsonFile(configPath);
//        //                using (var md5 = MD5.Create())
//        //                {
//        //                    var fingerprint = md5.ComputeHash(jsonBytes);

//        //                    if (_fingerprint != null && !fingerprint.SequenceEqual(_fingerprint))
//        //                    {
//        //                        var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
//        //                        previousToken.OnReload();
//        //                    }

//        //                    _fingerprint = fingerprint;
//        //                }
//        //            }
//        //            finally
//        //            {
//        //                Thread.Sleep(_checkForChangesInterval);
//        //            }
//        //        }

//        //    });

//        //    return _reloadToken;
//        //}
//    }
//}
