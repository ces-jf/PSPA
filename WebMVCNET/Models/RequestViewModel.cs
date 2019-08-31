using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVCNET.Models
{
    public class RequestViewModel
    {
        [Required]
        [Display(Name = "Endereço do Arquivo")]
        public string Url { get; set; }
        [Required]
        [Display(Name = "Nome da Base de Dados")]
        public string Index { get; set; }
    }
}
