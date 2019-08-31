using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infra.Entidades
{
    public class PedidoImportacao
    {
        public PedidoImportacao()
        {
            this.LogPedidoImportacao = new List<LogPedidoImportacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public DateTime? DataTermino { get; set; }
        [MaxLength(1)]
        public string Estado { get; set; }
        public string PastaTemp { get; set; }


        //Relationship
        public Usuario Usuario { get; set; }
        public ICollection<ArquivoBase> Arquivos { get; set; }
        public ICollection<LogPedidoImportacao> LogPedidoImportacao { get; set; }
    }
}
