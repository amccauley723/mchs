using System.Collections.Generic;
using System.Linq;
using MarshfieldClinic.Models;
using Examine;
using Examine.SearchCriteria;
using Umbraco.Web;
using System;
using Umbraco.Core.Models;

namespace MarshfieldClinic.Helpers
{
    /// <summary>
    /// A helper class giving you everything you need for searching with Examine.
    /// </summary>
    public class SearchHelper
    {
        private string _docTypeAliasFieldName { get { return "nodeTypeAlias"; } }
        private UmbracoHelper _uHelper { get; set; }

        /// <summary>
        /// Default constructor for SearchHelper
        /// </summary>
        /// <param name="uHelper">An umbraco helper to use in your class</param>
        public SearchHelper(UmbracoHelper uHelper)
        {
            _uHelper = uHelper;
        }

        /// <summary>
        /// Gets the search results model from the search term/// 
        /// </summary>
        /// <param name="searchModel">The search model with search term and other settings in it</param>
        /// <param name="allKeys">The form keys that were submitted</param>
        /// <returns>A SearchResultsModel object loaded with the results</returns>
        public SearchResultsModel GetSearchResults(SearchViewModel searchModel, string[] allKeys)
        {
            SearchResultsModel resultsModel = new SearchResultsModel();
            resultsModel.SearchTerm = searchModel.SearchTerm;
            resultsModel.PageNumber = GetPageNumber(allKeys);

            ISearchResults allResults = SearchUsingExamine(searchModel.DocTypeAliases.Split(','), searchModel.SearchGroups);
            resultsModel.TotalItemCount = allResults.TotalItemCount;
            resultsModel.Results = GetResultsForThisPage(allResults, resultsModel.PageNumber, searchModel.PageSize);

            resultsModel.PageCount = Convert.ToInt32(Math.Ceiling((decimal)resultsModel.TotalItemCount / (decimal)searchModel.PageSize));
            resultsModel.PagingBounds = GetPagingBounds(resultsModel.PageCount, resultsModel.PageNumber, searchModel.PagingGroupSize);
            return resultsModel;
        }

        /// <summary>
        /// Takes the examine search results and return the content for each page
        /// </summary>
        /// <param name="allResults">The examine search results</param>
        /// <param name="pageNumber">The page number of results to return</param>
        /// <param name="pageSize">The number of items per page</param>
        /// <returns>A collection of content pages for the page of results</returns>
        private IEnumerable<IPublishedContent> GetResultsForThisPage(ISearchResults allResults, int pageNumber, int pageSize)
        {
            return allResults.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(x => _uHelper.TypedContent(x.Id));
        }

        /// <summary>
        /// Performs a lucene search using Examine.
        /// </summary>
        /// <param name="documentTypes">Array of document type aliases to search for.</param>
        /// <param name="searchGroups">A list of search groupings, if you have more than one group it will apply an and to the search criteria</param>
        /// <returns>Examine search results</returns>
        public Examine.ISearchResults SearchUsingExamine(string[] documentTypes, List<SearchGroup> searchGroups)
        {
            ISearchCriteria searchCriteria = ExamineManager.Instance.CreateSearchCriteria(BooleanOperation.And);
            IBooleanOperation queryNodes = null;

            //only shows results for visible documents.
            queryNodes = searchCriteria.GroupedNot(new string[] { "umbracoNaviHide" }, "1");

            if (documentTypes != null && documentTypes.Length > 0)
            {
                //only get results for documents of a certain type
                queryNodes = queryNodes.And().GroupedOr(new string[] { _docTypeAliasFieldName }, documentTypes);
            }

            if (searchGroups != null && searchGroups.Any())
            {
                //in each search group it looks for a match where the specified fields contain any of the specified search terms
                //usually would only have 1 search group, unless you want to filter out further, i.e. using categories as well as search terms
                foreach (SearchGroup searchGroup in searchGroups)
                {
                    queryNodes = queryNodes.And().GroupedOr(searchGroup.FieldsToSearchIn, searchGroup.SearchTerms);
                }
            }

            //return the results of the search
            return ExamineManager.Instance.Search(queryNodes.Compile()); ;
        }

        /// <summary>
        /// Gets the page number from the form keys
        /// </summary>
        /// <param name="formKeys">All of the keys on the form</param>
        /// <returns>The page number</returns>
        public int GetPageNumber(string[] formKeys)
        {
            int pageNumber = 1;
            const string NAME_PREFIX = "page";
            const char NAME_SEPARATOR = '-';
            if (formKeys != null)
            {
                string pagingButtonName = formKeys.Where(x => x.Length > NAME_PREFIX.Length && x.Substring(0, NAME_PREFIX.Length).ToLower() == NAME_PREFIX).FirstOrDefault();
                if (!string.IsNullOrEmpty(pagingButtonName))
                {
                    string[] pagingButtonNameParts = pagingButtonName.Split(NAME_SEPARATOR);
                    if (pagingButtonNameParts.Length > 1)
                    {
                        if (!int.TryParse(pagingButtonNameParts[1], out pageNumber))
                        {
                            //pageNumber already set in tryparse
                        }
                    }
                }
            }
            return pageNumber;
        }

        /// <summary>
        /// Works out which pages the paging should start and end on
        /// </summary>
        /// <param name="pageCount">The number of pages</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="groupSize">The number of items per page</param>
        /// <returns>A PagingBoundsModel containing the paging bounds settings</returns>
        public PagingBoundsModel GetPagingBounds(int pageCount, int pageNumber, int groupSize)
        {
            int middlePageNumber = (int)(Math.Ceiling((decimal)groupSize / 2));
            int pagesBeforeMiddle = groupSize - (int)middlePageNumber;
            int pagesAfterMiddle = groupSize - (pagesBeforeMiddle + 1);
            int startPage = 1;
            if (pageNumber >= middlePageNumber)
            {
                startPage = pageNumber - pagesBeforeMiddle;
            }
            else
            {
                pagesAfterMiddle = groupSize - pageNumber;
            }
            int endPage = pageCount;
            if (pageCount >= (pageNumber + pagesAfterMiddle))
            {
                endPage = (pageNumber + pagesAfterMiddle);
            }
            bool showFirstButton = startPage > 1;
            bool showLastButton = endPage < pageCount;
            return new PagingBoundsModel(startPage, endPage, showFirstButton, showLastButton);
        }

    }
}