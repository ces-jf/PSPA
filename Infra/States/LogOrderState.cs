using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.States
{
    public class LogOrderState
    {
        public List<LogPedidoImportacao> Input { get; set; }
        public string SuccessReturn { get; set; }
        public IList<string> ErrorReturn { get; set; } = new List<string>();
    }
}
