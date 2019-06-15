using Data.Context;
using Infra.Entidades;
using Infra.Entidades.Identity;
using Infra.Interfaces;
using IoC.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            var server = configuration.GetValue<string>("ServerDatabase");
            var databaseName = configuration.GetValue<string>("DatabaseName");
            var user = configuration.GetValue<string>("UserDatabase");
            var password = configuration.GetValue<string>("PassDatabase");

            var connectionString = $"Server={server};Database={databaseName};User={user};Password={password}";

            
            services.AddDbContext<IdentityContext>(options =>

                options.UseMySql(connectionString, mysqlOptions =>
                {
                    mysqlOptions.ServerVersion(SystemHelper.Configuration.DatabaseVersion, ServerType.MySql);
                    mysqlOptions.MigrationsAssembly("WebApi");
                }));

            services.AddIdentity<Usuario, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
            configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions => {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions => {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;

                paramsValidation.ClockSkew = TimeSpan.Zero;
            });


            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            return services;
        }
    }
}