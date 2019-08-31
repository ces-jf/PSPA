using ContainerServices.Services.Interfaces;
using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerServices.Services.Implementation
{
    public class ImportacaoService : IImportacaoService
    {
        private IPedidoImportacaoBusiness _pedidoImportacao { get; set; }
        private IArquivoBaseBusiness _arquivoBase { get; set; }
        private IDictionary<string, Task> Tasks { get; set; }
        private IServiceProvider serviceProvider { get; set; }

        public ImportacaoService(IServiceProvider serviceProvider, IPedidoImportacaoBusiness pedidoImportacaoBusiness, IArquivoBaseBusiness arquivoBase)
        {
            this.serviceProvider = serviceProvider;
            this._arquivoBase = arquivoBase;
            this._pedidoImportacao = pedidoImportacaoBusiness;
            this.Tasks = new Dictionary<string, Task>();
        }

        public void OnStart()
        {
            var aguardandoDownload = _pedidoImportacao.GetPedidosAguardando();
            var aguardandoDownloadCount = aguardandoDownload.Count();

            if(aguardandoDownloadCount > 0)
            {
                for(var i = 0; i < aguardandoDownloadCount; i++)
                {
                    var pedido = aguardandoDownload.ElementAtOrDefault(i);

                    if (pedido == null)
                        continue;

                    var task = Task.Factory.StartNew(() => {

                        try
                        {
                            var context = serviceProvider.GetRequiredService<IIdentityContext>();

                            var arquivos = pedido.Arquivos;

                            if (arquivos.Count == 1)
                            {
                                var arquivo = arquivos.FirstOrDefault();
                                //_arquivoBase.DownloadOnDisk(arquivo, pedido, context);
                                //var files = _arquivoBase.CheckFileList(arquivo.Index.Name, pedido, context);

                                _arquivoBase.UpdateToRegisterData(pedido, context);
                            }
                        }
                        catch(Exception erro)
                        {
                            return;
                        }
                    });
                }
            }
        }
    }
}
