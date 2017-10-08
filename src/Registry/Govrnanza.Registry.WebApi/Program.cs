using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Govrnanza.Extensions.Configuration;

namespace Govrnanza.Registry.WebApi
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var appRootPath = Directory.GetCurrentDirectory();
            BuildWebHost(appRootPath, args).Run();
        }

        public static IWebHost BuildWebHost(string appRootPath, string[] args)
        {
            var webHostBuilder = GetWebHostBuilder(appRootPath, args);
            return webHostBuilder.Build();
        }


        public static IWebHostBuilder GetWebHostBuilder(string appRootPath, string[] args)
        {
            var webHostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(appRootPath)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var secretsMode = GetSecretsMode(hostingContext.HostingEnvironment);
                    config.AddGovrnanzaConfig(secretsMode, "REGISTRY_CONFIG_FILE");
                })
                .UseStartup<Startup>();

            return webHostBuilder;
        }

        private static SecretsMode GetSecretsMode(IHostingEnvironment env)
        {
            if (env.IsProduction())
                return SecretsMode.DockerSecrets;

            var useDockerSecrets = Environment.GetEnvironmentVariable("REGISTRY_USE_DOCKER_SECRETS");
            if (useDockerSecrets != null && useDockerSecrets.Equals("false", StringComparison.OrdinalIgnoreCase))
                return SecretsMode.LocalFile;

            return SecretsMode.DockerSecrets;
        }
    }
}
