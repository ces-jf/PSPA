using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class Value
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [Required]
        public string Descricao { get; set; }

        // Relationships
        public Cabecalho Cabecalho { get; set; }
        public ArquivoBase ArquivoBase { get; set; }
    }
}
