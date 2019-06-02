using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Infra.Entidades;
using Infra.Entidades.Identity;
using IoC.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SystemHelper;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.LoginModel;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly Configuration _configuration;
        public AuthController(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] InputModel inputModel, [FromServices]UserManager<Usuario> _userManager, [FromServices] SignInManager<Usuario> _signInManager, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            bool isValidCredentials = false;
            if( inputModel != null && !string.IsNullOrWhiteSpace(inputModel.Email))
            {
                var userIdentity = await _userManager.FindByEmailAsync(inputModel.Email);
                if(userIdentity != null)
                {
                    var resultLogin = await _signInManager.CheckPasswordSignInAsync(userIdentity, inputModel.Password, false);
                    if(resultLogin.Succeeded)
                    {
                        isValidCredentials = true;
                    }
                }
            }

            if (isValidCredentials)
            {
                ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(inputModel.Email, "Login"), new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Email, inputModel.Email)
                });


                var creationDate = DateTime.Now;
                var expireDate = creationDate + TimeSpan.FromMinutes(_configuration.TokenMinutesValidation);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = creationDate,
                    Expires = expireDate
                });
                var token = handler.WriteToken(securityToken);

                return new ObjectResult(new
                {
                    authenticated = true,
                    created = creationDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    expiration = expireDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    accessToken = token,
                    message = "Autenticado."
                });
            }
            else
            {
                return BadRequest(new
                {
                    authenticated = false,
                    message = "Falha ao Autenticar"
                });
            }
        }

        [AllowAnonymous]
        [Route("/Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario, [FromServices]UserManager<Usuario> _userManager, [FromServices] SignInManager<Usuario> _signInManager, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            if(ModelState.IsValid)
            {
                var user = usuario;
                var result = await _userManager.CreateAsync(usuario, usuario.Password);

                if(result.Succeeded)
                {
                    var inputModel = new InputModel
                    {
                        Email = user.Email,
                        Password = user.Password
                    };

                    return await this.Login(inputModel, _userManager, _signInManager, signingConfigurations, tokenConfigurations);
                }
                else
                {
                    return BadRequest(new { error = result.Errors.Select(a => a.Description) });
                }
            }

            return BadRequest(new { error = "Objeto passado não válido." });

        }
    }
}
