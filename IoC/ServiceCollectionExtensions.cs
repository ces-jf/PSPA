using Data.Class;
using Data.Context;
using Infra.Business.Classes;
using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Interfaces;
using Infra.States;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http;
using SystemHelper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IArquivoBaseBusiness, ArquivoBaseBusiness>();
            services.AddTransient<IPedidoImportacaoBusiness, PedidoImportacaoBusiness>();
            services.AddTransient<IIndexBusiness, IndexBusiness>();
            services.AddTransient<IdentityBusiness, IdentityBusiness>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IIdentityContext, IdentityContext>();
            services.AddScoped<LoginState>();
            services.AddScoped<RegisterState>();
            services.AddScoped<RequestedOrdersState>();
            services.AddScoped<RequestOrderState>();
            services.AddScoped<QueryDataState>();
            services.AddScoped<ModalFilterState>();
            services.AddScoped<ModalSelectState>();
            services.AddScoped<LogOrderState>();
            services.AddScoped<RequestAdjustImport>();

            return services;
        }
    }
}
