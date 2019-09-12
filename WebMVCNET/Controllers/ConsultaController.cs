using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Consultar(string indexBase)
        {
            try
            {
                var selectFilter = new List<string> {
                    "NOME MUNICÍPIO",
                    "CÓDIGO MUNICÍPIO SIAFI",
                    "NOME FAVORECIDO"
                };

                //var export = this.ArquivoBaseBusiness.ConsultaToCSV(indexBase,selectFilter);
                var export = this.ArquivoBaseBusiness.ConsultaToCSV(indexBase);

                return File(export.ExportToBytes(), "text/csv", $"{indexBase}.csv");
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }
    }
}