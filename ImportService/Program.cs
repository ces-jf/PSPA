using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemHelper;
using SystemHelper.Configurations;
using SystemHelper.ViewModel;

namespace ImportService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                    var configuration = builder.Build(); services.AddOptions();

                    services.Configure<Configuration>(configuration.GetSection("GeneralConfigurations"));
                    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
                    services.Configure<ElasticSearch>(configuration.GetSection("ElasticSearch"));

                    services.AddHostedService<Worker>();
                    services.AddRestIndentity(configuration);
                    services.AddDependencyInjection();
                });
    }
}
