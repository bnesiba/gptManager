using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions
{
    public class InternetSearch : ToolDefinition
    {
        public string Name => "InternetSearch";

        public string Description => "Get additional information from the internet using google search.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "query",
                type = "string",
                description = "The string to send to the search engine"
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
        {
            return new OpenAIToolMessage("stuff", toolCall.id);
        }
    }
}