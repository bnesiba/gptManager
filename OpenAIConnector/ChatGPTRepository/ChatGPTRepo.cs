using System.Text;
using Newtonsoft.Json;
using OpenAIConnector.ChatGPTRepository.models;

namespace OpenAIConnector.ChatGPTRepository
{
    public class ChatGPTRepo
    {
        private readonly HttpClient _httpClient;
        //TODO: get from config?
        private Uri baseUrl = new Uri("https://api.openai.com/v1/");

        public ChatGPTRepo(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            //TODO: get from config?
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "<Bearer Token Goes Here>");

        }

        public OpenAIModelsResponse? GetModels()
        {
            var response = _httpClient.GetAsync(baseUrl + "models").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent.ReadAsStringAsync().Result;
                var models = JsonConvert.DeserializeObject<OpenAIModelsResponse>(responseString);
                return models;
            }
            else
            {
                return null;
            }
        }

        public OpenAIChatResponse? SimpleChat(string message)
        {
            var chatRequest = new OpenAIChatRequest()
            {
                //model = "gpt-4-turbo-preview",
                model = "gpt-3.5-turbo",
                messages = new List<OpenAIChatMessage>
                {
                    //gaslighting chatgpt for fun...
                    //new OpenAISystemMessage("system", "You are a math tutor. You teach math good."),
                    //new OpenAIUserMessage("does 1+1 = 3?"),
                    //new OpenAIAssistantMessage("assistant", "yes, 1+1 = 3."),
                    new OpenAIUserMessage(message)
                }
            };
            return Chat(chatRequest);
        }

        public OpenAIChatResponse? AdvancedChat(string systemMessage, string userMessage,
            List<OpenAIChatMessage> currentMessages)
        {
            var chatRequest = new OpenAIChatRequest()
            {
                //model = "gpt-4-turbo-preview",
                model = "gpt-3.5-turbo",
                messages = currentMessages
            };
            return Chat(chatRequest);
        }

        public OpenAIChatResponse? Chat(OpenAIChatRequest chatRequest)
        {
            var json = JsonConvert.SerializeObject(chatRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(baseUrl + "chat/completions", content).Result;
            var responseContent1 = response.Content.ReadAsStringAsync().Result;


            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                //var responseString = responseContent.ReadAsStringAsync().Result;
                var responseString = responseContent1;
                var chatResponse = JsonConvert.DeserializeObject<OpenAIChatResponse>(responseString);
                return chatResponse;
            }
            else
            {
                //var responseContent = response.Content.ReadAsStringAsync().Result;
                //TODO: should probably throw here?
                return null;
            }
        }

        //TODO: find a way to avoid having to use the image request
        public OpenAIChatResponse? Chat(OpenAIImageChatRequest chatRequest)
        {
            var json = JsonConvert.SerializeObject(chatRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(baseUrl + "chat/completions", content).Result;
            var responseContent1 = response.Content.ReadAsStringAsync().Result;


            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                var responseString = responseContent1;
                var chatResponse = JsonConvert.DeserializeObject<OpenAIChatResponse>(responseString);
                return chatResponse;
            }
            else
            {
                //TODO: should probably throw here?
                return null;
            }
        }
    }
}
