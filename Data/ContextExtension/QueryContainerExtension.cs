using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ContextExtension
{
    public static class QueryContainerExtension
    {
        public static QueryContainer GetFilter(this QueryContainerDescriptor<Dictionary<string, string>> temp, string filterField, string filterType, string filterValue)
        {
            double.TryParse(filterValue, out double filterValueCast);

            switch (filterType)
            {
                case ">":
                    return temp.Range(range => range.Field(filterField).GreaterThan(filterValueCast));
                case ">=":
                    return temp.Range(range => range.Field(filterField).GreaterThanOrEquals(filterValueCast));
                case "<":
                    return temp.Range(range => range.Field(filterField).LessThan(filterValueCast));
                case "<=":
                    return temp.Range(range => range.Field(filterField).LessThanOrEquals(filterValueCast));
                case "=":
                    return temp.Match(match => match.Field(filterField).Query(filterValue));
                default:
                    return temp.Match(match => match.Field(filterField).Query(filterValue));
            }
        }

        public static QueryContainer FilterMatch(this QueryContainerDescriptor<Dictionary<string, string>> query, IEnumerable<Tuple<string, string, string>> filterFilter = null)
        {
            var filterReturn = new QueryContainerDescriptor<Dictionary<string, string>>();
            var resultQuery = new QueryContainer();

            if (filterFilter == null)
                return filterReturn.MatchAll();

            foreach (var filter in filterFilter)
            {
                var temp = new QueryContainerDescriptor<Dictionary<string, string>>();

                if (filter.Item3.Contains(","))
                {
                    var splitContainer = new QueryContainer();
                    var itens = filter.Item3.Split(',');

                    foreach (var item in itens)
                    {
                        splitContainer = splitContainer || temp.GetFilter(filter.Item1, filter.Item2, item);
                    }

                    resultQuery = resultQuery && (splitContainer);
                    continue;
                }

                resultQuery = resultQuery && temp.GetFilter(filter.Item1, filter.Item2, filter.Item3);
            }

            return resultQuery;
        }
    }
}
