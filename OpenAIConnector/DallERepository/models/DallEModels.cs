using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAIConnector.DallERepository.models
{
    public class DallEGenerationRequest
    {
        public string model {  get; set; }
        public string prompt { get; set; }
        public int n {  get; set; }
        public string size { get; set; }
        public string? quality { get; set; }
        public string? style { get; set; }
        public string? user { get; set; }

        public DallEGenerationRequest(string input)
        {
            model = "dall-e-3";
            prompt = input;
            n = 1;
            size = "1024x1024";
            quality = "hd";//"hd" or null (must be null for dall-e-2)
            style = "vivid"; //"vivid" or "natural" (or null - must be null for dall-e-2) 
            user = "Admin";
        }

    }

    public class DallEGenerationResponse
    {
        //public DateTime created { get; set; }
        public List<DallEImageData> data { get; set; }
    }

    public class DallEImageData
    {
        public string revised_prompt { get; set; }
        public string url { get; set; }
    }
}
