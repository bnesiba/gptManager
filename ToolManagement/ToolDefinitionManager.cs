using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement
{
    //TODO: does this still need to exist? probably not.
    //maybe if I end up actually having multiple sets of tools?
    public class ToolDefinitionManager
    {
       private List<IToolDefinition> tools = new List<IToolDefinition>();

       public ToolDefinitionManager(IEnumerable<IToolDefinition> definedTools)
       {
            tools = definedTools.ToList();
       }

        public OpenAITool[] GetToolDefinitions()
        {
            return tools.Select(t => t.GetToolRequestDefinition()).ToArray();
        }   
    }
}
