using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions;

namespace ToolManagement
{
    public class ToolDefinitionManager
    {
        public OpenAITool[] GetToolDefinitions()
        {
            return new List<OpenAITool>()
            {
                UserQuery.GetToolRequestDefinition(),
                FindEmails.GetToolRequestDefinition(),
                InternetSearch.GetToolRequestDefinition()
            }.ToArray();
        }

        //public OpenAITool[] GetTestingStuff()
        //{
        //    return new List<OpenAITool>()
        //    {
        //        new OpenAITool()
        //        {
        //            type = "function",
        //            function = new OpenAiToolFunction()
        //            {
        //                description = "Get additional information directly from the user.",
        //                name = "UserQuery",
        //                parameters = new
        //                {
        //                    type = "object",
        //                    properties = new {
        //                        location = new
        //                        {
        //                            type = "string",
        //                            description = "The city and state, e.g. San Francisco, CA"
        //                        },
        //                        unit = new
        //                        {
        //                            type = "string",
        //                            @enum = new string[] { "celsius", "fahrenheit" }
        //                        }
        //                    }
        //                }
        //            }
        //        },  
        //    }.ToArray();
        //}
    }
}
