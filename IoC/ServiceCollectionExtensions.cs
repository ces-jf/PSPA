using Data.Class;
using Data.Context;
using Infra.Business.Classes;
using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IArquivoBaseBusiness, ArquivoBaseBusiness>();
            services.AddTransient<IPedidoImportacaoBusiness, PedidoImportacaoBusiness>();
            services.AddTransient<IdentityBusiness, IdentityBusiness>();
            services.AddScoped<IIdentityContext, IdentityContext>();

            return services;
        }
    }
}
