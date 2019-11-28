using Infra.Entidades;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Business.Classes.Identity
{
    public class IdentityBusiness: IDisposable
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityBusiness(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Usuario> GetUsuarioAsync(ClaimsPrincipal User, bool includeRoles = false)
        {
            try
            {
                var usuario = await _userManager.GetUserAsync(User);

                if (!includeRoles)
                    return usuario;

                var roles = await _userManager.GetRolesAsync(usuario);

                if (roles.Count > 0)
                    usuario.Roles.AddRange(roles);

                return usuario;
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(role);
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            if (users.Count > 0)
                throw new Exception("Have users assigned in this role.");

            var role = await _roleManager.FindByNameAsync(roleName);
            await _roleManager.DeleteAsync(role);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~IdentityBusiness()
        // {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
