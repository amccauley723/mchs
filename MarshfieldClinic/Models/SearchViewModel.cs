using System.Collections.Generic;

namespace MarshfieldClinic.Models
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public string DocTypeAliases { get; set; }
        public string FieldPropertyAliases { get; set; }
        public int PageSize { get; set; }
        public int PagingGroupSize { get; set; }
        public List<SearchGroup> SearchGroups { get; set; }
        public SearchResultsModel SearchResults { get; set; }
    }
}