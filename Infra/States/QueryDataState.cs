using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.States
{
    public class QueryDataState
    {
        public List<Tuple<SearchBase, string, string>> Input = new List<Tuple<SearchBase, string, string>>();
        public Dictionary<string, List<Cabecalho>> InputCabecalho = new Dictionary<string, List<Cabecalho>>();
        public List<SearchBase> SelectedIndex = new List<SearchBase>();
        public IList<Dictionary<string, string>> GraphicValues = new List<Dictionary<string, string>>();
        public long? NumberEntries { get; set; }
        public bool AllEntries { get; set; }
    }

    public class SearchBase: ICloneable
    {

        public string Name { get; set; }
        public IList<Filter> FilterColumns { get; set; } = new List<Filter>();
        public IList<string> SelectColumns { get; set; } = new List<string>();

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
