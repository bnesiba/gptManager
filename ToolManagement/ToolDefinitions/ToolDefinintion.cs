using OpenAIConnector.ChatGPTRepository.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolManagement.ToolDefinitions
{
    public interface IToolDefinition
    {
        public string Name { get; }
        public string Description { get; }

        public List<ToolProperty> InputParameters { get; }

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall);

    }

    public static class ToolDefinitionExtensions
    {
        public static OpenAITool GetToolRequestDefinition(this IToolDefinition toolDefinition)
        {
            OpenAiToolFunction toolFunction = new OpenAiToolFunction()
            {
                description = toolDefinition.Description,
                name = toolDefinition.Name,
                parameters = new
                {
                    type = "object",
                    properties = toolDefinition.InputParameters.ToDictionary(p => p.name, p => new { type = p.type, description = p.description })
                }
            };

            return new OpenAITool()
            {
                type = "function",
                function = toolFunction
            };
        }

        public static Dictionary<string, string>? GetToolRequestParameters(this IToolDefinition toolDefinition,
            OpenAIToolCall toolCall)
        {
            Dictionary<string, string>? requestParameters = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(toolCall.function.arguments);
            return requestParameters;
        }

        public static bool RequestArgumentsValid(this IToolDefinition toolDefinition,
            Dictionary<string, string>? requestParameters)
        {
            if (requestParameters == null)
            {
                return false;
            }

            foreach (var parameter in toolDefinition.InputParameters)
            {
                if (parameter.IsRequired && !requestParameters.ContainsKey(parameter.name))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
