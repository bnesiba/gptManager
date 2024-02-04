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
       private List<ToolDefinition> tools = new List<ToolDefinition>();

       public ToolDefinitionManager()
       {
            tools.Add(new UserQuery());
            tools.Add(new InternetSearch());
            tools.Add(new FindEmails());
            tools.Add(new SendEmail());
       }

        public OpenAITool[] GetToolDefinitions()
        {
            return tools.Select(t => t.GetToolRequestDefinition()).ToArray();
        }

        public List<OpenAIToolMessage> ExecuteTools(List<OpenAIChatMessage> chatContext, List<OpenAIToolCall> toolCalls)
        {
            var invalidToolCalls = toolCalls.Where(t => tools.All(tf => tf.Name != t.function.name)).ToList();
            if (invalidToolCalls.Any())
            {
                //throw new Exception($"Invalid tool call(s): {string.Join(", ", invalidToolCalls.Select(t => t.function.name))}");
                Console.WriteLine($"WARNING There were invalid tool calls:{string.Join(", ", invalidToolCalls.Select(t => t.function.name))}");
            }   

            var results = new List<OpenAIToolMessage>();
            foreach (var toolCall in toolCalls)
            {
                var tool = tools.FirstOrDefault(t => t.Name == toolCall.function.name);
                if (tool != null)
                {
                    results.Add(tool.ExecuteTool(chatContext, toolCall));
                }
            }
            return results;
        }   
    }
}
