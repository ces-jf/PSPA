using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class LinhaPedidoImportacao
    {
        public LinhaPedidoImportacao()
        {
            this.EstaFeito = "N";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string EstaFeito { get; set; }

        //Relationship
        public ArquivoBase ArquivoBase { get; set; }
        public PedidoImportacao PedidoImportacao { get; set; }
    }
}
