using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Docs
{
    internal class SwaggerHelper
    {
        internal static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            AddSwaggerDocPerVersion(swaggerGenOptions, webApiAssembly);
            ApplyDocInclusions(swaggerGenOptions);

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach(var xmlFile in xmlFiles)
                swaggerGenOptions.IncludeXmlComments(xmlFile);

            swaggerGenOptions.DescribeAllEnumsAsStrings();
            swaggerGenOptions.OperationFilter<FormatXmlCommentProperties>();
        }

        private static void AddSwaggerDocPerVersion(SwaggerGenOptions swaggerGenOptions, Assembly webApiAssembly)
        {
            var apiVersionDescriptions = new ApiVersionDescriptions();
            var mdPath = Path.Combine("Docs", "ApiVersion1Description.md");
            apiVersionDescriptions.AddDescription("1", File.ReadAllText(mdPath));

            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                swaggerGenOptions.SwaggerDoc($"v{apiVersion}",
                    new Info
                    {
                        Title = "Govrnanza Registry",
                        Version = $"v{apiVersion}",
                        Description = apiVersionDescriptions.GetDescription(apiVersion),
                        Contact = new Contact()
                        {
                            Name = "Govrnanza",
                            Url = "https://github.com/Vanlightly/Govrnanza"
                        }
                    });
            }
        }

        private static void ApplyDocInclusions(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.DocInclusionPredicate((docName, apiDesc) =>
            {
                var versions = apiDesc.ControllerAttributes()
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions.Any(v => $"v{v.ToString()}" == docName);
            });
        }

        private static IEnumerable<string> GetApiVersions(Assembly webApiAssembly)
        {
            var apiVersion = webApiAssembly.DefinedTypes
                .Where(x => x.IsSubclassOf(typeof(Controller)) && x.GetCustomAttributes<ApiVersionAttribute>().Any())
                .Select(y => y.GetCustomAttribute<ApiVersionAttribute>())
                .SelectMany(v => v.Versions)
                .Distinct()
                .OrderByDescending(x => x);

            return apiVersion.Select(x => x.ToString());
        }

        internal static void ConfigureSwagger(SwaggerOptions swaggerOptions)
        {
            swaggerOptions.RouteTemplate = "api-docs/{documentName}/swagger.json";
        }

        internal static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
        {
            var webApiAssembly = Assembly.GetEntryAssembly();
            var apiVersions = GetApiVersions(webApiAssembly);
            foreach (var apiVersion in apiVersions)
            {
                swaggerUIOptions.SwaggerEndpoint($"v{apiVersion}/swagger.json", $"V{apiVersion} Docs");
            }
            swaggerUIOptions.RoutePrefix = "api-docs";
            swaggerUIOptions.ShowRequestHeaders();
            swaggerUIOptions.ShowJsonEditor();
            swaggerUIOptions.InjectStylesheet("theme-feeling-blue-v2.css");
            swaggerUIOptions.InjectOnCompleteJavaScript("CustomisedSwagger.js");
        }
    }
}
