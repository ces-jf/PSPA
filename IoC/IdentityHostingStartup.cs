using Data.Context;
using Infra.Entidades;
using Infra.Entidades.Identity;
using Infra.Interfaces;
using IoC.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceIdentityCollectionExtensions
    {
        public static IServiceCollection AddRestIndentity(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("DatabaseConfiguration");

            var server = configurationSection.GetValue<string>("ServerDatabase");
            var databaseName = configurationSection.GetValue<string>("DatabaseName");
            var user = configurationSection.GetValue<string>("UserDatabase");
            var password = configurationSection.GetValue<string>("PassDatabase");

            var connectionString = $"Server={server};Database={databaseName};User={user};Password={password}";

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<IdentityContext>(options =>

                options.UseMySql(connectionString, mysqlOptions =>
                {
                    mysqlOptions.ServerVersion(SystemHelper.Configuration.DatabaseVersion, ServerType.MySql);
                    mysqlOptions.MigrationsAssembly("WebMVCNET");
                }));

            services.AddIdentity<Usuario, Role>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();



            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(120);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            return services;
        }
    }
}