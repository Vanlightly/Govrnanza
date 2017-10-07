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
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Creates the IWebHost
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            var logFactory = new LoggerFactory()
                    .AddConsole(LogLevel.Debug)
                    .AddDebug();

            var logger = logFactory.CreateLogger<Program>();
            logger.LogInformation("Starting " + Environment.MachineName);

            var webHostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var secretsMode = GetSecretsMode(hostingContext.HostingEnvironment);
                    config.AddGovrnanzaConfig(secretsMode, "REGISTRY_CONFIG_FILE");
                })
                .UseStartup<Startup>()
                .Build();

            logger.LogInformation("Host built");

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
