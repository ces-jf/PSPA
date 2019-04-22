using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.Entidades
{
    public class ArquivoBase
    {
        [Key]
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
