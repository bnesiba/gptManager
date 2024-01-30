using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions
{

    //this ended up being an odd place to start because it involves returning and waiting for a response...
    public class UserQuery
    {

        public static OpenAITool GetToolRequestDefinition()
        {
            OpenAiToolFunction toolFunction = new OpenAiToolFunction()
            {

                name = "UserQuery",
                description = "Get additional information directly from the user.",
                parameters = new
                {
                    type = "object",
                    properties = new {
                        query = new
                        {
                            type = "string",
                            description = "The question to ask the user."
                        }
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
            return new OpenAIAssistantMessage("UserQueryTool",  query);
        }
    }
}
