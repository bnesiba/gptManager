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

        public WikipediaSearchOrchestrator(GoogleCustomWikiSearchConnector searchConnector)
        {
            _searchConnector = searchConnector;
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
            var searchResults = _searchConnector.Search(query, count)?.items ?? new List<WikipediaSearchResult>();

            foreach (var wikiPage in searchResults)
            {
                //get the content of the wiki page
                //summarize the content keeping the search query/purpose in mind
                //determine if all results are needed/helpful
                //return relevant summarized results
            }

            return null;
        }
    }
}
