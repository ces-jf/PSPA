using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Infra.Entidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorSite
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public string ReturnUrl { get; set; }

        public async Task<IActionResult>
            OnGetAsync([FromServices] SignInManager<Usuario> _signInManager,[FromServices] UserManager<Usuario> _userManager,string Email, string Password, bool RememberMe = false)
        {
            string returnUrl = Url.Content("~/");
            List<string> errorList = new List<string>();

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var userIdentity = await _userManager.FindByEmailAsync(Email);
                if (userIdentity != null)
                {
                    var resultLogin = await _signInManager.PasswordSignInAsync(userIdentity, Password, RememberMe, false);
                    if (resultLogin.Succeeded)
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        errorList.Add("Invalid login attempt.");
                        var erro = string.Join(Environment.NewLine, errorList);
                        returnUrl = $"~/AccountLogin/{erro}";
                        return LocalRedirect(returnUrl);
                    }
                }
            }
            errorList.Add("Invalid login attempt.");
            var error = string.Join(Environment.NewLine, errorList);
            returnUrl = $"~/AccountLogin/{error}";
            return LocalRedirect(returnUrl);
        }
    }
}