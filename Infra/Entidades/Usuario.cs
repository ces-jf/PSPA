using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infra.Entidades
{
    // Add profile data for application users by adding properties to the Usuario class
    public class Usuario : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
    }
}
