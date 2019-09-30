using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVCNET.Models
{
    public class BaseBuscaViewModel
    {
        public string Name { get; set; }
        public long NumberEntries { get; set; }
        public bool AllEntries { get; set; }
        public IEnumerable<ColunaBase> ColumnsFilter { get; set; }
        public IEnumerable<ColunaBase> ColumnsSelect { get; set; }

        public IEnumerable<ColunaBase> ColumnsGroup { get; set; }
    }

    public class ColunaBase
    {
        public string Descricao { get; set; }
        public string FilterType { get; set; }
        public string ValueFilter { get; set; }
    }
}
