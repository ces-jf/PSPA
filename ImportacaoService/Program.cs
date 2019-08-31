using ContainerServices.Services.Implementation;
using ContainerServices.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using SystemHelper;

namespace ContainerServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddDependencyInjection();
            services.AddRestIndentity(configuration);
            services.AddOptions();
            services.Configure<Configuration>(configuration.GetSection("GeneralConfigurations"));

            //IoC de Serviços
            services.AddScoped<IImportacaoService, ImportacaoService>();

            var serviceProvider = services.BuildServiceProvider();

            //Configurando Serviços
            var serviceImportacao = serviceProvider.GetService<IImportacaoService>();

            //Startando Serviços
            serviceImportacao.OnStart();
        }
    }
}
