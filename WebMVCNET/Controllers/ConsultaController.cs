using System;
using System.Collections.Generic;
using System.Linq;
using Infra.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVCNET.Models;
using SystemHelper.NetCoreTagHelper;
using SystemHelper.NetCoreMVCAttribute;

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

            if(baseBusca.ColumnsSelect != null)
                if(baseBusca.ColumnsSelect.Count() > 0)
                    selectFilter = baseBusca.ColumnsSelect.Select(a => a.Descricao);

            try
            {
                var fileName = this.ArquivoBaseBusiness.ConsultaToCSV(User, indexBase, selectFilter);
                var guid = Guid.NewGuid().ToString();

                TempData.Put(guid, new Tuple<string, string>(fileName, $"{indexBase}.csv"));

                return Ok(new { fileGuid = guid });
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }

        [HttpGet]
        [DeleteFile]
        public IActionResult DownloadFile(string guid)
        {
            var file = TempData.Get<Tuple<string, string>>(guid);
            var fileStream = System.IO.File.OpenRead(file.Item1);
            return File(fileStream, "text/csv", file.Item2);
        }
    }
}