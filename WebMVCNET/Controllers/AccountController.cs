using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SystemHelper;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.LoginModel;

namespace WebMVCNET.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly Configuration _configuration;

        public AccountController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, IOptions<Configuration> configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public RegisterModel.InputModel RegisterInput { get; set; }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (Input != null && !string.IsNullOrWhiteSpace(Input.Email))
                {
                    var userIdentity = await _userManager.FindByEmailAsync(Input.Email);
                    if (userIdentity != null)
                    {
                        var resultLogin = await _signInManager.PasswordSignInAsync(userIdentity, Input.Password, false, false);
                        if (resultLogin.Succeeded)
                        {
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View();
                        }
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new Usuario { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }
    }
}