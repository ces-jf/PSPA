using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.States
{
    public class ModalSelectState
    {
        public string IndexName { get; set; }
        public string SelectToAddSelect { get; set; }
        public IList<Header> Columns { get; set; } = new List<Header>();
        public IList<string> SelectColumns { get; set; } = new List<string>();
    }
}
