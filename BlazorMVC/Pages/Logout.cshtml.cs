using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorSite
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync([FromServices] SignInManager<Usuario> _signInManager)
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect(Url.Content("~/"));
        }
    }
}