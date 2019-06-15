using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class PedidoImportacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public DateTime? DataTermino { get; set; }


        //Relationship
        public Usuario Usuario { get; set; }
        public ICollection<LogPedidoImportacao> LogPedidoImportacao { get; set; }
    }
}
