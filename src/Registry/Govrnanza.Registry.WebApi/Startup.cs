using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using Govrnanza.Registry.WebApi.Docs;
using Govrnanza.Registry.Backend.Infrastructure.Database;
using MediatR;
using Govrnanza.Registry.Backend.Responses;
using Govrnanza.Registry.WebApi.Secrets;

namespace Govrnanza.Registry.WebApi
{
    /// <summary>
    /// Configures the application on start up
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
            LoggerFactory = loggerFactory;
            StartupLogger = loggerFactory.CreateLogger("Startup");
        }

        /// <summary>
        /// Configuration abstraction
        /// </summary>
        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public ILoggerFactory LoggerFactory { get; }

        public ILogger StartupLogger { get; }
        
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RegistryDbContext>(opt =>
                    opt.UseSqlServer(SecretsResolver.ResolveEmbeddedSecret(HostingEnvironment.EnvironmentName, Configuration,  Configuration.GetConnectionString("Registry"))));

            services.AddMediatR(typeof(Startup).Assembly, typeof(GetResult).Assembly);

            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

            services.AddMvc().AddJsonOptions(j =>
            {
                j.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                j.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSwagger(SwaggerHelper.ConfigureSwagger);
            app.UseSwaggerUI(SwaggerHelper.ConfigureSwaggerUI);

            app.UseMvc();
        }
    }
}
