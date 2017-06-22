using System.Web.Mvc;
using Umbraco.Web.Mvc;
using MarshfieldClinic.Models;
using Umbraco.Web;
using MarshfieldClinic.Helpers;
using System.Collections.Generic;

namespace CodeShare.Web.Controllers
{
    public class SearchController : SurfaceController
    {
        #region Private Variables and Methods

        private SearchHelper _searchHelper { get { return new SearchHelper(new UmbracoHelper(UmbracoContext.Current)); } }

        private string PartialViewPath(string name)
        {
            return $"~/Views/Partials/Search/{name}.cshtml";
        }

        private List<SearchGroup> GetSearchGroups(SearchViewModel model)
        {
            List<SearchGroup> searchGroups = null;
            if (!string.IsNullOrEmpty(model.FieldPropertyAliases))
            {
                searchGroups = new List<SearchGroup>();
                searchGroups.Add(new SearchGroup(model.FieldPropertyAliases.Split(','), new string[] { model.SearchTerm }));
            }
            return searchGroups;
        }

        #endregion

        #region Controller Actions

        [HttpGet]
        public ActionResult RenderSearchForm(string query, string docTypeAliases, string fieldPropertyAliases, int pageSize, int pagingGroupSize)
        {
            SearchViewModel model = new SearchViewModel();
            if (!string.IsNullOrEmpty(query))
            {
                model.SearchTerm = query;
                model.DocTypeAliases = docTypeAliases;
                model.FieldPropertyAliases = fieldPropertyAliases;
                model.PageSize = pageSize;
                model.PagingGroupSize = pagingGroupSize;
                model.SearchGroups = GetSearchGroups(model);
                model.SearchResults = _searchHelper.GetSearchResults(model, Request.Form.AllKeys);
            }
            return PartialView(PartialViewPath("_SearchForm"), model);
        }



        [HttpPost]
        public ActionResult SubmitSearchForm(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.SearchTerm))
                {
                    model.SearchTerm = model.SearchTerm;
                    model.SearchGroups = GetSearchGroups(model);
                    model.SearchResults = _searchHelper.GetSearchResults(model, Request.Form.AllKeys);
                }
                return RenderSearchResults(model.SearchResults);
            }
            return null;
        }

        public ActionResult RenderSearchResults(SearchResultsModel model)
        {
            return PartialView(PartialViewPath("_SearchResults"), model);
        }

        #endregion

    }
}