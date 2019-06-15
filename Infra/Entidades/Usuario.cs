using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Infra.Entidades
{
    // Add profile data for application users by adding properties to the Usuario class
    public class Usuario : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string SecondName { get; set; }


        //Relationships
        public ICollection<PedidoImportacao> PedidosImportacao { get; set; }
    }
}
