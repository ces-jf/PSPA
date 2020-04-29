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
        public string UrlOrigem { get; set; }

        //Relationships
        [Required]
        public Index Index { get; set; }
        public ICollection<Header> Headers { get; set; }
        public PedidoImportacao PedidoImportacao { get; set; }
        //public ICollection<Value> Values { get; set; }
    }
}
