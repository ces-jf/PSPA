using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.Entidades
{
    public class PedidoImportacao
    {
        [Key]
        public long ID { get; set; }
        public DateTime DataTermino { get; set; }


        //Relationship
        public Usuario Usuario { get; set; }
    }
}
