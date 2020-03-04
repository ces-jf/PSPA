using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.States
{
    public class RequestedOrdersState
    {
        public List<PedidoImportacao> Input { get; set; } = new List<PedidoImportacao>();
        public string SuccessReturn { get; set; }
        public IList<string> ErrorReturn { get; set; } = new List<string>();
    }
}
