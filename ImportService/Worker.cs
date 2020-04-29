using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infra.Business.Interfaces;
using Infra.Enums;
using Infra.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImportService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var serviceScope = serviceProvider.CreateScope();
                using var orderBusiness = serviceScope.ServiceProvider.GetRequiredService<IPedidoImportacaoBusiness>();
                var pendingOrders = orderBusiness.GetPendingOrders();
                var pendingOrdersQuery = pendingOrders.AsParallel();
                pendingOrdersQuery.ForAll(item => {
                    List<bool> resultSuccess = new List<bool>();

                    using var fileBusiness = serviceScope.ServiceProvider.GetRequiredService<IArquivoBaseBusiness>();
                    var filesToImport = item.Arquivos.Where(a => !a.Nome.Contains("inicial.zip"));
                    foreach(var fileToImport in filesToImport)
                    {
                        resultSuccess.Add(fileBusiness.InsertFile(item.ID, fileToImport.ID));
                    }

                    if (resultSuccess.Contains(false))
                        orderBusiness.SetStatus(item.ID, OrderState.Error);
                    else
                        orderBusiness.SetStatus(item.ID, OrderState.Imported);
                });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
