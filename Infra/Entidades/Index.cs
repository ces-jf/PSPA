using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Entidades
{
    public class Index
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public ICollection<ArquivoBase> ArquivoBases { get; set; }
    }
}
