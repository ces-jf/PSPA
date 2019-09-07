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
        [Display(Name = "File Address")]
        public string Url { get; set; }
        [Required]
        [Display(Name = "Database Name")]
        public string Index { get; set; }
    }
}
