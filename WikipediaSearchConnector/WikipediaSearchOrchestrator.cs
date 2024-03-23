using AIUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WikipediaSearchConnector.Models;

namespace WikipediaSearchConnector
{
    public class WikipediaSearchOrchestrator
    {
        private GoogleCustomWikiSearchConnector _searchConnector;
        private WikipediaConnector _wikiConnector;
        private ArticleSummarizer _articleSummarizer;

        public WikipediaSearchOrchestrator(GoogleCustomWikiSearchConnector searchConnector,WikipediaConnector wikiConnector, ArticleSummarizer articleSummarizer)
        {
            _searchConnector = searchConnector;
            _wikiConnector = wikiConnector;
            _articleSummarizer = articleSummarizer;
        }

        /// <summary>
        /// Search wikipedia.org using a custom google search and then using wikipedia.org API to get the content of the wiki page and summarize it
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <param name="additionalContext"></param>
        /// <returns></returns>
        public string? Search(string query, int count = 3, string additionalContext = null)
        {
            //find relevent wiki pages
            var searchResults = _searchConnector.Search(query, count)?.items ?? new List<GoogleCustomSearchResult>();

            foreach (var wikiPage in searchResults)
            {
                //get the content of the wiki page
                var wikiExtract = _wikiConnector.GetExtracts(wikiPage.GetArticleTitle());
                //summarize the content keeping the search query/purpose in mind
                //determine if all results are needed/helpful
                //return relevant summarized results
            }

            return null;
        }
    }
}
