using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WikipediaSearchConnector.Models
{
    public class WikipediaExtractsResponse
    {
        public WikipediaQuery query { get; set; }
    }

    public class WikipediaQuery
    {
        public Dictionary<string, WikipediaPage> pages { get; set; }
    }

    public class WikipediaPage
    {
        public string title { get; set; }
        public string extract { get; set; }
    }

    public static class GoogleWikiSearchExtensions
    {
        public static string GetArticleTitle(this GoogleCustomSearchResult searchResult)
        {
            int domainLabelStart = searchResult.title.IndexOf(" - Wikipedia", StringComparison.Ordinal);
            return domainLabelStart > 0 ? searchResult.title.Substring(0, domainLabelStart) : searchResult.title;
        }
    }
}
