using OpenAIConnector.ChatGPTRepository.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolManagement.ToolDefinitions
{
    public interface ToolDefinition
    {
        public string Name { get; }
        public string Description { get; }

        public List<ToolProperty> InputParameters { get; }

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall);

    }

    public static class ToolDefinitionExtensions
    {
        public static OpenAITool GetToolRequestDefinition(this ToolDefinition toolDefinition)
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
    }
}
