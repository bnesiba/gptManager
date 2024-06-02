using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement.ToolDefinitions
{
    public class NewsSearch : IToolDefinition
    {
        public string Name => "NewsSearch";

        public string Description => "Search NEWS SITES for NEWS articles - ONLY USE FOR NEWS";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "query",
                type = "string",
                description = "Query string to find news related to",
                IsRequired = true
            }

        };

        //TODO: abstract more of this out? everything except the actual call and the response object is shared across tools
        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
        {
            Dictionary<string, string>? requestParameters = this.GetToolRequestStringParameters(toolCall);
            if (requestParameters != null)
            {
                bool toolCallArgumentsValid = this.RequestArgumentsValid(requestParameters);

                if (toolCallArgumentsValid)
                {
                    //process query here
                    
                    var outputObject = new
                    {
                        query = requestParameters["query"],
                        resultCount = 3,
                        results = new List<object>()
                        {
                            new
                            {
                                sourceURI = "https://www.google.com",
                                title = "North Korean scientists discover alien ship on moon, hurried efforts for contact",
                            },
                            new
                            {
                                sourceURI = "https://www.google.com",
                                title = "US Amateur Astronomer claims alien invasion immanent",
                            },
                            new
                            {
                                sourceURI = "https://www.google.com",
                                title = "NASA pushes back on 'Moon men' rumors, we've been there, we would know",
                            },
                        }

                    };
                    //return new OpenAIToolMessage($"InternetSearchResponse: " + JsonSerializer.Serialize(outputObject),this.Name, toolCall.id);
                    return new OpenAIToolMessage($"InternetSearchResponse:" + JsonSerializer.Serialize(outputObject), toolCall.id);
                }
                return new OpenAIToolMessage("ERROR: Arguments to 'InternetSearchResponse' tool were invalid or missing", toolCall.id);
            }

            return new OpenAIToolMessage("ERROR: No Arguments were provided", toolCall.id);
        }

        //new and improved (simplified) tool call 
        //TODO: Eventually remove the other one
        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            //process query here

            var outputObject = new
            {
                query = toolParams.GetStringParam("query"),
                resultCount = 3,
                results = new List<object>()
                        {
                            new
                            {
                                sourceURI = "https://www.google.com",
                                title = "North Korean scientists discover alien ship on moon, hurried efforts for contact",
                            },
                            new
                            {
                                sourceURI = "https://www.google.com",
                                title = "US Amateur Astronomer claims alien invasion immanent",
                            },
                            new
                            {
                                sourceURI = "https://www.google.com",
                                title = "NASA pushes back on 'Moon men' rumors, we've been there, we would know",
                            },
                        }

            };
            return new OpenAIToolMessage($"InternetSearchResponse:" + JsonSerializer.Serialize(outputObject), toolParams.ToolRequestId);
        }
    }
}