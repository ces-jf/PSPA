using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.Entidades
{
    public class Usuario
    {
        [Key]
        public long Matricula { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string SobreNome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }


        //Relationship
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
