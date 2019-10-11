using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using SystemHelper;
using WebMVCNET.Models;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.LoginModel;

namespace WebMVCNET.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel.InputModel input, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

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
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel input, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = input.UserName,
                    Email = input.Email,
                    FirstName = input.FirstName,
                    SecondName = input.SecondName
                };

                var result = await _userManager.CreateAsync(user, input.Password);

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

        public async Task<IActionResult> LogOut()
        {
            try
            {
                await this._signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromServices]IEmailSender _emailSender, ForgotPasswordModel.InputModel ForgotPasswordInput)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ForgotPasswordInput.Email);

                if (user == null)
                    return View("ForgotPasswordConfirmation");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(ForgotPasswordInput.Email, "Reset Password",
                   "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(ForgotPasswordInput);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel.InputModel ResetPasswordInput)
        {
            if (!ModelState.IsValid)
            {
                return View(ResetPasswordInput);
            }
            var user = await _userManager.FindByEmailAsync(ResetPasswordInput.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, ResetPasswordInput.Code, ResetPasswordInput.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count > 0)
                user.Roles.AddRange(roles);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Usuario userModified)
        {
            if (!ModelState.IsValid)
                return View("Manage", userModified);

            var user = await _userManager.FindByIdAsync(userModified.Id);

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Not possible to reconize logged user.");
                return View("Manage", userModified);
            }

            user.UserName = userModified.UserName;
            user.FirstName = userModified.FirstName;
            user.SecondName = userModified.SecondName;
            user.Email = userModified.Email;

            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded)
            {
                AddErrors(result);
                return View("Manage", userModified);
            }

            return View("Manage",user);
        }

        [HttpGet]
        public IActionResult Accounts()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ViewUser([FromServices] RoleManager<Role> roleManager, string idUser)
        {
            var user = await _userManager.FindByIdAsync(idUser);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count > 0)
                user.Roles.AddRange(roles);

            ViewData["SystemRoles"] = roleManager.Roles.Where(a => !roles.Contains(a.Name)).Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id
            }).ToList();

            return View("Manage", user);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
