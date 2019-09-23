using System;
using System.Collections.Generic;
using System.Linq;
using Infra.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebMVCNET.Models;
using SystemHelper.NetCoreTagHelper;
using SystemHelper.NetCoreMVCAttribute;
using Microsoft.AspNetCore.Authorization;

namespace WebMVCNET.Controllers
{
    [Authorize]
    public class QueryController : Controller
    {
        //IoC Properties
        private IArquivoBaseBusiness ArquivoBaseBusiness { get; set; }

        public QueryController(IArquivoBaseBusiness arquivoBaseBusiness)
        {
            this.ArquivoBaseBusiness = arquivoBaseBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Run(BaseBuscaViewModel baseBusca)
        {
            var indexBase = baseBusca.Name;
            IEnumerable<string> selectFilter = null;
            IEnumerable<Tuple<string, string, string>> filterFilter = null;

            if(baseBusca.ColumnsSelect != null)
                if(baseBusca.ColumnsSelect.Count() > 0)
                    selectFilter = baseBusca.ColumnsSelect.Select(a => a.Descricao);

            if (baseBusca.ColumnsFilter != null)
                if (baseBusca.ColumnsFilter.Count() > 0)
                    filterFilter = baseBusca.ColumnsFilter.Select(a => new Tuple<string, string, string>(a.Descricao, a.FilterType, a.ValueFilter));

            try
            {
                var fileName = this.ArquivoBaseBusiness.ConsultaToCSV(User, indexBase, selectFilter, filterFilter, baseBusca.NumberEntries, baseBusca.AllEntries);
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