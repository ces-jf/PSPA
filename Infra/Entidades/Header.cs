using Infra.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class Header
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public HeaderType HeaderType { get; set; }
        
        //Relationships
        public ArquivoBase ArquivoBase { get; set; }
    }
}
