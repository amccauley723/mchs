using System.Web.Mvc;
using Umbraco.Web.Mvc;
using MarshfieldClinic.Models;
using System.Collections.Generic;
using Examine;
using System;
using Examine.LuceneEngine.SearchCriteria;

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


    }
}