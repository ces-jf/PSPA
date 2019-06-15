using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class LogPedidoImportacao
    {
        public LogPedidoImportacao()
        {
            this.DataCriacao = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [MaxLength(1)]
        public string IndicadorStatus { get; set; }
        public DateTime DataCriacao { get; set; }

        //Relationships
        [Required]
        public PedidoImportacao PedidoImportacao { get; set; }
    }
}
