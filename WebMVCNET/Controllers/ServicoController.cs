using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebMVCNET.Controllers
{
    public class ServicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}