using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Central_De_Erros.Configurations
{
    public static class SwaggerConfig
    {

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {

            
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(name: "v1", new Info
                {
                    Title = "Error Center",
                    Version = "v1",
                    Description = "This Web API was created to centralize error logs in applications",
                    Contact = new Contact()
                    {
                        Name = "Gislaine Mendes da Rocha",
                        Url = "https://www.linkedin.com/in/gislaine-mendes-da-rocha/"
                    }
                });
                

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } },
                };

                opt.AddSecurityDefinition(
                    "Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Copie 'Bearer ' + token'",
                        Name = "Authorization",
                        Type = "apiKey"
                    });

                opt.AddSecurityRequirement(security);
            });
            return services;
        }
    }
}
