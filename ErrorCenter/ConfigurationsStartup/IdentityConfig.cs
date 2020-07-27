using Central_De_Erros.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_De_Erros.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {   
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<CentralDeErrosContext>();

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = appSettings.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = appSettings.Audience,
                        ValidateAudience = true                       
                    };
                });

            return services;
        }
    }
}
