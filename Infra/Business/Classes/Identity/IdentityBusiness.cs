using Infra.Entidades;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Business.Classes.Identity
{
    public class IdentityBusiness
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public IdentityBusiness(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Usuario> LoginAsync(Usuario user)
        {
            bool isAuthenticated = false;
            Usuario userIdentity;

            if(user != null && !string.IsNullOrEmpty(user.Email))
            {
                userIdentity = await _userManager.FindByNameAsync(user.Email);

                if(userIdentity != null)
                {
                    var loginResult = await _signInManager.CheckPasswordSignInAsync(userIdentity, user.Password, false);

                    if(loginResult.Succeeded)
                    {
                        return userIdentity;
                    }


                }
            }

            userIdentity = new Usuario();
            return userIdentity;

        }

        public async Task<Usuario> CreateUserAsync(Usuario user)
        {
            var result = await _userManager.CreateAsync(user, user.Password);
            if(result.Succeeded)
            {
                user = await this.LoginAsync(user);
                return user;
            }
            
            foreach(var error in result.Errors)
            {

            }

            user = new Usuario();

            return user;
        }
    }
}
