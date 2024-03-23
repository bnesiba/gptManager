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
    public class GoogleCustomWikiSearchConnector
    {
        private readonly HttpClient _httpClient;
        //TODO: get from config?
        private Uri baseUrl = new Uri("https://www.googleapis.com/customsearch/v1?key=AIzaSyC8_mTwBqU0ArSkVRy2oprKQ6f9W6Ubca4&cx=85da1f98ddbbb4828");

        public GoogleCustomWikiSearchConnector(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        public GoogleCustomSearchResponse? Search(string query, int count = 10)
        {
            var response = _httpClient.GetAsync(baseUrl + "&q=" + query + "&num=" + count).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent.ReadAsStringAsync().Result;
                var searchResponse = JsonConvert.DeserializeObject<GoogleCustomSearchResponse>(responseString);
                return searchResponse;
            }
            else
            {
                return null;
            }
        }
    }
}