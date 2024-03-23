using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaSearchConnector.Models
{
    public class GoogleCustomSearchResponse
    {
        public List<GoogleCustomSearchResult> items { get; set; }
    }

    public class GoogleCustomSearchResult
    {
        public string title { get; set; }
        public Uri link { get; set; }
        public string snippet { get; set; }
    }
}
