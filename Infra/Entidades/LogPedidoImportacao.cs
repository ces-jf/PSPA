using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.Entidades
{
    public class LogPedidoImportacao
    {
        [Key]
        public long ID { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [MaxLength(1)]
        public string IndicadorStatus { get; set; }

        //Relationships
        [Required]
        public PedidoImportacao PedidoImportacao { get; set; }
    }
}
