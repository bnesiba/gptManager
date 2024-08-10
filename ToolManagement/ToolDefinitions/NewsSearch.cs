using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace ToolManagement.ToolDefinitions
{
    public class NewsSearch : IToolDefinition
    {
        //static accessor for Tool Management
        public static string ToolName => "NewsSearch";

        public string Name => ToolName;

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