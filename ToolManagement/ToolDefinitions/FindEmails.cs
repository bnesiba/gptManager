using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions
{
    public class FindEmails
    {

        public static OpenAITool GetToolRequestDefinition()
        {
            OpenAiToolFunction toolFunction = new OpenAiToolFunction()
            {
                description = "Get the latest emails that contain the search string or from address",
                name = "FindEmails",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        SearchString = new
                        {
                            type = "string",
                            description = "Used to search the body of the email"
                        },
                        FromAddress = new
                        {
                            type = "string",
                            description = "The from address of the emails to search for"
                        },
                    }
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
            return new OpenAIAssistantMessage("FindEmailsTool", query);
        }
    }
}