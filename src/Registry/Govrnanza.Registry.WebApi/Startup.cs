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
using Govrnanza.Registry.WebApi.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using Govrnanza.Registry.WebApi.Infrastructure.Database;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using Govrnanza.Registry.WebApi.Docs;

namespace Govrnanza.Registry.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBusinessDomainsService, BusinessDomainsService>();
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<ITagService, TagService>();

            services.AddDbContext<RegistryDbContext>(opt =>
                   opt.UseSqlServer(Configuration.GetConnectionString("Registry")));

            services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

            services.AddMvc().AddJsonOptions(j =>
            {
                j.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                j.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(SwaggerHelper.ConfigureSwagger);
            app.UseSwaggerUI(SwaggerHelper.ConfigureSwaggerUI);

            app.UseMvc();
        }
    }
}
