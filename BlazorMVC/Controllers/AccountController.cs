using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Route("signin")]
        public async Task<IActionResult> Signin([FromServices] UserManager<Usuario> _userManager, [FromServices] SignInManager<Usuario> _signInManager, [FromBody] LoginModel.InputModel input)
        {
            if (ModelState.IsValid)
            {
                if (input != null && !string.IsNullOrWhiteSpace(input.Email))
                {
                    var userIdentity = await _userManager.FindByEmailAsync(input.Email);
                    if (userIdentity != null)
                    {
                        var resultLogin = await _signInManager.PasswordSignInAsync(userIdentity, input.Password, false, false);
                        if (resultLogin.Succeeded)
                        {
                            return Ok();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return BadRequest(ModelState);
                        }
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }
    }
}