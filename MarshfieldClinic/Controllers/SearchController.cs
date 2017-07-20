using System.Web.Mvc;
using Umbraco.Web.Mvc;
using MarshfieldClinic.Models;
using System.Collections.Generic;
using Examine;
using System;
using Examine.LuceneEngine.SearchCriteria;
using MarshfieldClinic.Helpers;
using Umbraco.Web;

namespace MarshfieldClinic.Web.Controllers
{
    public class SearchController : SurfaceController
    {
        public JsonResult LookFor(string d, string t)
        {
            if (!string.IsNullOrEmpty(t))
            {
                var criteria = ExamineManager.Instance
                    .SearchProviderCollection["ExternalSearcher"]
                    .CreateSearchCriteria();

                // Find pages that contain our search text in either their nodeName or bodyText fields...
                // but exclude any pages that have been hidden.
                // searchCriteria.Fields("nodeName",terms.Boost(8)).Or().Field("metaTitle","hello".Boost(5)).Compile();

                var crawl = criteria.GroupedOr(new string[] { "nodeName", "specialties", "servicesList", "locationTypes", "synonyms" }, t.MultipleCharacterWildcard())
                    .And().NodeTypeAlias(d)
                    .Not()
                    .Field("umbracoNaviHide", "1")
                    .Compile();

                ISearchResults SearchResults = ExamineManager.Instance
                    .SearchProviderCollection["ExternalSearcher"]
                    .Search(crawl);

                IList<Result> results = new List<Result>();

                foreach (SearchResult sr in SearchResults)
                {
                    string niceUrl = Umbraco.NiceUrl(sr.Id);
                    Result result = new Result()
                    {
                        Id = sr.Id,
                        Score = (int)Math.Min(sr.Score * 100, 100),
                        Url = sr.Fields["urlName"],
                        Name = sr.Fields["nodeName"],
                        NiceUrl = niceUrl
                    };

                    results.Add(result);
                }

                return Json(results, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("Search term not found", JsonRequestBehavior.AllowGet);
            }

        }

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
                //searchGroups.Add(new SearchGroup(model.FieldPropertyAliases.Split(','), new string[] { model.SearchTerm }));
                searchGroups.Add(new SearchGroup(model.FieldPropertyAliases.Split(','), model.SearchTerm.Split()));
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