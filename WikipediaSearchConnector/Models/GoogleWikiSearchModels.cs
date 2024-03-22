using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaSearchConnector.Models
{
    public class WikipediaSearchResponse
    {
        public List<WikipediaSearchResult> items { get; set; }
    }

    public class WikipediaSearchResult
    {
        public string title { get; set; }
        public Uri link { get; set; }
        public string snippet { get; set; }
    }
}
