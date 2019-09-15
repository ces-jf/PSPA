using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVCNET.Models;
using SystemHelper.NetCoreTagHelper;

namespace WebMVCNET.Controllers
{
    public class ConsultaController : Controller
    {
        //IoC Properties
        private IArquivoBaseBusiness ArquivoBaseBusiness { get; set; }

        public ConsultaController(IArquivoBaseBusiness arquivoBaseBusiness)
        {
            this.ArquivoBaseBusiness = arquivoBaseBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Consultar(BaseBuscaViewModel baseBusca)
        {
            var indexBase = baseBusca.Name;
            IEnumerable<string> selectFilter = null;

            if(baseBusca.ColumnsSelect != null || baseBusca.ColumnsSelect.Count() > 0)
                selectFilter = baseBusca.ColumnsSelect.Select(a => a.Descricao);

            try
            {
                var export = this.ArquivoBaseBusiness.ConsultaToCSV(indexBase, selectFilter);
                var guid = Guid.NewGuid().ToString();

                TempData.Put(guid, new Tuple<byte[], string>(export.ExportToBytes(), $"{indexBase}.csv"));

                return Ok(new { fileGuid = guid });
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }

        public IActionResult DownloadFile(string guid)
        {
            var file = TempData.Get<Tuple<byte[], string>>(guid);
            return File(file.Item1, "text/csv", file.Item2);
        }
    }
}