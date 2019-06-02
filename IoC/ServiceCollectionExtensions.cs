using Data.Class;
using Data.Context;
using Infra.Business.Classes;
using Infra.Business.Interfaces;
using Infra.Class;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection DBConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            var server = configuration.GetValue<string>("ServerDatabase");
            var databaseName = configuration.GetValue<string>("DatabaseName");
            var user = configuration.GetValue<string>("UserDatabase");
            var password = configuration.GetValue<string>("PassDatabase");

            var connectionString = $"Server={server};Database={databaseName};User={user};Password={password}";

            services.AddDbContext<IdentityContext>(options => options.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.ServerVersion(SystemHelper.Configuration.DatabaseVersion, ServerType.MariaDb);
                mysqlOptions.MigrationsAssembly("WebApi");
            }));

            return services;
        }

        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IArquivoBaseBusiness, ArquivoBaseBusiness>();

            return services;
        }
    }
}
