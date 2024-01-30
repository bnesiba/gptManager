using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions
{
    public class InternetSearch
    {

        public static OpenAITool GetToolRequestDefinition()
        {
            OpenAiToolFunction toolFunction = new OpenAiToolFunction()
            {
                description = "Get additional information from the internet using google search.",
                name = "InternetSearch",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        query = new
                        {
                            type = "string",
                            description = "The string to send to the search engine"
                        }
                    },
                }

            };

            return new OpenAITool()
            {
                type = "function",
                function = toolFunction
            };
        }

        public static OpenAIChatMessage GetToolMessage(string query)
        {
            return new OpenAIAssistantMessage("InternetSearchTool", query);
        }
    }
}