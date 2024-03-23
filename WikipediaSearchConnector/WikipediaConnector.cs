using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaSearchConnector.Models;

namespace WikipediaSearchConnector
{
    public class WikipediaConnector
    {
        private readonly HttpClient _httpClient;
        //TODO: get from config?
        private Uri baseUrl = new Uri("https://en.wikipedia.org/w/api.php?format=json&action=query");

        public WikipediaConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public WikipediaExtractsResponse? GetExtracts(string title, bool detailed = false)
        {
            var response = _httpClient.GetAsync(baseUrl + $"&prop=extracts{(detailed ? "&exintro" : "")}&explaintext&redirects=1&titles=" + title).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent.ReadAsStringAsync().Result;
                var extracts = JsonConvert.DeserializeObject<WikipediaExtractsResponse>(responseString);
                return extracts;
            }
            else
            {
                return null;
            }
        }



    }
}
