using Infra.Business.Interfaces;
using Infra.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebMVCNET.Models;

namespace WebMVCNET.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        //Forms bind properties
        [BindProperty]
        public RequestViewModel RequestViewModel { get; set; }

        //IoC Properties
        private IArquivoBaseBusiness ArquivoBaseBusiness { get; set; }
        private IServiceProvider ServiceProvider { get; set; }

        public OrderController(IArquivoBaseBusiness arquivoBaseBusiness, IServiceProvider serviceProvider)
        {
            this.ArquivoBaseBusiness = arquivoBaseBusiness;
            this.ServiceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public new IActionResult Request()
        {
            return View();
        }

        public IActionResult Log(long id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new async Task<IActionResult> Request(string onPost = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var pedidoImportacao = await this.ArquivoBaseBusiness.CreateImportRequestAsync(RequestViewModel.Url, RequestViewModel.Index, User);
                var arquivo = pedidoImportacao.Arquivos.FirstOrDefault();

                if (arquivo != null)
                {
                    _ = Task.Factory.StartNew(() =>
                    {
                        Thread.CurrentThread.Name = $"Thread de Carga de Base Pedido {pedidoImportacao.ID}";

                        var serviceScope = this.ServiceProvider.CreateScope();
                        var context = serviceScope.ServiceProvider.GetRequiredService<IIdentityContext>();

                        context.PedidoImportacao.Attach(pedidoImportacao);

                        this.ArquivoBaseBusiness.DownloadOnDisk(arquivo, pedidoImportacao, context);
                        this.ArquivoBaseBusiness.CheckFileList(arquivo.Index.Name, pedidoImportacao, context);
                        this.ArquivoBaseBusiness.UpdateToRegisterData(pedidoImportacao, context);

                        this.ArquivoBaseBusiness.InserirArquivo(pedidoImportacao, context);

                        serviceScope.Dispose();
                    });
                }
                TempData["Success"] = $"Pedido para carga da base de dados {pedidoImportacao.ID} cadastrada com sucesso!";
                return View();
            }
            catch (Exception erro)
            {
                ModelState.AddModelError("Erro gerado no servidor", erro.Message);
                return View();
            }
        }
    }
}