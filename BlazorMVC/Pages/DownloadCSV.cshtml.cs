using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SystemHelper.Extensions;
using SystemHelper.NetCoreMVCAttribute;

namespace BlazorSite
{
    [DeleteFile]
    public class DownloadCSVModel : PageModel
    {
        public IActionResult OnGet(string fileName, string fileDownloadName)
        {
            var fileStream = System.IO.File.OpenRead(fileName);
            return File(fileStream, "text/csv", fileDownloadName);
        }
    }
}