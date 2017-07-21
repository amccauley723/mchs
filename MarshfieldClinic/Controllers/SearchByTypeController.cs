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
    public class SearchByTypeController : SurfaceController
    {
        public JsonResult Doctors(string t)
        {
            if (!string.IsNullOrEmpty(t))
            {
                var criteria = ExamineManager.Instance
                    .SearchProviderCollection["ExternalSearcher"]
                    .CreateSearchCriteria();

                // Find pages that contain our search text in either their nodeName or bodyText fields...
                // but exclude any pages that have been hidden.
                // searchCriteria.Fields("nodeName",terms.Boost(8)).Or().Field("metaTitle","hello".Boost(5)).Compile();
                Examine.SearchCriteria.IBooleanOperation crawl = null;
                string[] splitSearch = t.Split(' ');
                int i = 0;
                for (i = 0; i < splitSearch.Length; i++)
                {
                    crawl = criteria.GroupedOr(new string[] { "nodeName", "specialties", "servicesList", "locationTypes", "synonyms", "dXCodes", "additionalProcedures" }, splitSearch[i].MultipleCharacterWildcard())
                   .And().NodeTypeAlias("provider")
                   .Not()
                   .Field("umbracoNaviHide", "1");
                }



                ISearchResults SearchResults = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"].Search(crawl.Compile());

                IList<Doctors> results = new List<Doctors>();

                foreach (SearchResult sr in SearchResults)
                {
                    string niceUrl = Umbraco.NiceUrl(sr.Id);
                    Doctors result = new Doctors()
                    {
                        Id = sr.Id,
                        Score = (int)Math.Min(sr.Score * 100, 100),
                        Url = sr.Fields["urlName"],
                        Name = sr.Fields["nodeName"],
                        NiceUrl = niceUrl,
                        NodeAliasType = sr.Fields["nodeTypeAlias"]
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

        public JsonResult Locations(string t)
        {
            if (!string.IsNullOrEmpty(t))
            {
                var criteria = ExamineManager.Instance
                    .SearchProviderCollection["ExternalSearcher"]
                    .CreateSearchCriteria();

                // Find pages that contain our search text in either their nodeName or bodyText fields...
                // but exclude any pages that have been hidden.
                // searchCriteria.Fields("nodeName",terms.Boost(8)).Or().Field("metaTitle","hello".Boost(5)).Compile();
                Examine.SearchCriteria.IBooleanOperation crawl = null;
                string[] splitSearch = t.Split(' ');
                int i = 0;
                for (i = 0; i < splitSearch.Length; i++)
                {
                    crawl = criteria.GroupedOr(new string[] { "nodeName", "servicesList", "zipCode", "locationTypes" }, splitSearch[i].MultipleCharacterWildcard())
                   .And().NodeTypeAlias("location")
                   .Not()
                   .Field("umbracoNaviHide", "1");
                }



                ISearchResults SearchResults = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"].Search(crawl.Compile());

                IList<Locations> results = new List<Locations>();

                foreach (SearchResult sr in SearchResults)
                {
                    string niceUrl = Umbraco.NiceUrl(sr.Id);
                    Locations result = new Locations()
                    {
                        Id = sr.Id,
                        Score = (int)Math.Min(sr.Score * 100, 100),
                        Url = sr.Fields["urlName"],
                        Name = sr.Fields["nodeName"],
                        NiceUrl = niceUrl,
                        NodeAliasType = sr.Fields["nodeTypeAlias"]
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

        public JsonResult Specialties(string t)
        {
            if (!string.IsNullOrEmpty(t))
            {
                var criteria = ExamineManager.Instance
                    .SearchProviderCollection["ExternalSearcher"]
                    .CreateSearchCriteria();

                // Find pages that contain our search text in either their nodeName or bodyText fields...
                // but exclude any pages that have been hidden.
                // searchCriteria.Fields("nodeName",terms.Boost(8)).Or().Field("metaTitle","hello".Boost(5)).Compile();
                Examine.SearchCriteria.IBooleanOperation crawl = null;
                string[] splitSearch = t.Split(' ');
                int i = 0;
                for (i = 0; i < splitSearch.Length; i++)
                {
                    crawl = criteria.GroupedOr(new string[] { "nodeName", "specialty", "availableAtLocations" }, splitSearch[i].MultipleCharacterWildcard())
                   .And().NodeTypeAlias("providerType")
                   .Not()
                   .Field("umbracoNaviHide", "1");
                }



                ISearchResults SearchResults = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"].Search(crawl.Compile());

                IList<Specialties> results = new List<Specialties>();

                foreach (SearchResult sr in SearchResults)
                {
                    string niceUrl = Umbraco.NiceUrl(sr.Id);
                    Specialties result = new Specialties()
                    {
                        Id = sr.Id,
                        Score = (int)Math.Min(sr.Score * 100, 100),
                        Url = sr.Fields["urlName"],
                        Name = sr.Fields["nodeName"],
                        NiceUrl = niceUrl,
                        NodeAliasType = sr.Fields["nodeTypeAlias"]
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

    }
}