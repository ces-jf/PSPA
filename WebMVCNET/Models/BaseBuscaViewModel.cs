using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVCNET.Models
{
    public class BaseBuscaViewModel
    {
        public string Name { get; set; }
        public IEnumerable<ColunaBase> ColumnsFilter { get; set; }
        public IEnumerable<ColunaBase> ColumnsSelect { get; set; }
    }

    public class ColunaBase
    {
        public string Descricao { get; set; }
    }
}
