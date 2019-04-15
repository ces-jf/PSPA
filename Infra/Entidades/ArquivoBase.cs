using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Entidades
{
    public class ArquivoBase
    {
        public Index Index { get; set; }
        public IList<string> Cabecalho { get; set; }
        public IList<string> Values { get; set; }
    }
}
