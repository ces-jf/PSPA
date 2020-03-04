using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.States
{
    public class ModalFilterState
    {
        public string IndexName { get; set; }
        public string SelectToAddFilter { get; set; }
        public IList<Cabecalho> Columns { get; set; } = new List<Cabecalho>();
        public IList<Filter> FilterColumns = new List<Filter>();
    }

    public class Filter
    {
        public Filter(string name, string type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
