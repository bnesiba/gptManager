using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaSearchConnector.Models;

namespace WikipediaSearchConnector
{
    internal class WikipediaConnector
    {
        private readonly HttpClient _httpClient;
        //TODO: get from config?
        private Uri baseUrl = new Uri("https://en.wikipedia.org/w/api.php?format=json&action=query");

        public WikipediaConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

    }
}
