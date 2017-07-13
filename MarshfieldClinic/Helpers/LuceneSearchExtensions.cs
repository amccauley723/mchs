using System;
using System.Security;
using Examine.SearchCriteria;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System.Linq;

namespace Examine.LuceneEngine.SearchCriteria
{
    public static class LuceneSearchExtensions
    {

        public static IBooleanOperation OrderBy(this IQuery qry, params SortableField[] fields)
        {
            return qry.OrderBy(
                fields.Select(x => x.FieldName + "[Type=" + x.SortType.ToString().ToUpper() + "]").ToArray());
        }

        public static IBooleanOperation OrderByDescending(this IQuery qry, params SortableField[] fields)
        {
            return qry.OrderByDescending(
                fields.Select(x => x.FieldName + "[Type=" + x.SortType.ToString().ToUpper() + "]").ToArray());
        }

    }
}