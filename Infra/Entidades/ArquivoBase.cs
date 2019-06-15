using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class ArquivoBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public Index Index { get; set; }


        //Relationships
        public ICollection<Cabecalho> Cabecalhos { get; set; }
        //public ICollection<Value> Values { get; set; }
    }
}
