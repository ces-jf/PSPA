using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infra.States
{
    public class QueryDataState
    {
        public List<Tuple<SearchBase, string, string>> Input = new List<Tuple<SearchBase, string, string>>();
        public Dictionary<string, List<Header>> InputCabecalho = new Dictionary<string, List<Header>>();
        public List<SearchBase> SelectedIndex = new List<SearchBase>();
        public IQueryable<GraphicSearch> GraphicValues;
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

    public class GraphicSearch
    {
        public GraphicSearch(string column, string value)
        {
            this.Column = column;
            this.Value = float.Parse(value);
        }

        public GraphicSearch(KeyValuePair<string, string> keyValuePair): this(keyValuePair.Key, keyValuePair.Value) { }

        public string Column { get; set; }
        public float Value { get; set; }
    }
}
