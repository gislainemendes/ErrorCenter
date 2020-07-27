using Central_De_Erros.ViewModel;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
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
    public static class EmailServiceConfig
    {

        public static IServiceCollection AddEmailServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(configuration.GetSection("AuthMessageSenderOptions"));

            return services;
        }
    }
}
