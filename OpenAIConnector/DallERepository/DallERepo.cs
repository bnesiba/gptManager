using Newtonsoft.Json;
using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.DallERepository.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAIConnector.DallERepository
{
    public  class DallERepo
    {
        private readonly HttpClient _httpClient;
        //TODO: get from config?
        private Uri baseUrl = new Uri("https://api.openai.com/v1/");

        public DallERepo(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            //TODO: get from config?
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "<token goes here>");

        }

        public DallEGenerationResponse? GenerateImage(DallEGenerationRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(baseUrl+"images/generations", content).Result;
            var responseContent1 = response.Content.ReadAsStringAsync().Result;

            if(response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent1;
                var imageResponse = JsonConvert.DeserializeObject<DallEGenerationResponse>(responseString);
                return imageResponse;
            }
            else
            {
                //should probably throw here?
                return null;
            }
        }
    }
}
