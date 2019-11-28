using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Business.Classes.Identity;
using Infra.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebMVCNET.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private IServiceProvider ServiceProvider { get; set; }

        public RoleController(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var roles = new List<IdentityRole>();

            using(var identityBusiness = this.ServiceProvider.GetService<IdentityBusiness>())
            {
                roles.AddRange(identityBusiness.GetRoles());
            }

            return View(roles);
        }

        public async Task<IActionResult> Delete(string roleName)
        {
            using(var identityBusiness = this.ServiceProvider.GetService<IdentityBusiness>())
            {
                try
                {
                    await identityBusiness.DeleteRoleAsync(roleName);
                    return RedirectToAction("Index");
                }
                catch(Exception erro)
                {
                    ModelState.AddModelError(string.Empty, erro.Message);
                    return RedirectToAction("Index");
                }

            }
        }
    }
}
