using AutoMapper;
using Central_De_Erros.Models;
using Central_De_Erros.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Central_De_Erros.Configurations
{
    public static class ContextConfig
    {
        public static IServiceCollection addContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CentralDeErrosContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<CentralDeErrosContext>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IErrorRepository, ErrorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFrequencyRepository, FrequencyRepository>();

            return services;
        }
    }
}
