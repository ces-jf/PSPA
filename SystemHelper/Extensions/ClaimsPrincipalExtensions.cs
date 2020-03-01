using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SystemHelper.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? claim.Value : null;
        }
    }
}
