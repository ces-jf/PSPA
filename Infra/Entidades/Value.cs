using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.Entidades
{
    public class Value
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Descricao { get; set; }

        // Relationships
        public Cabecalho Cabecalho { get; set; }
        public ArquivoBase ArquivoBase { get; set; }
    }
}
