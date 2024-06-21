using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement
{
    //TODO: does this still need to exist? probably not here.
    //maybe if I end up actually having multiple sets of tools?
    public class ToolDefinitionManager
    {
        private List<IToolDefinition> tools = new List<IToolDefinition>();

        //TODO: define other tool sets?
        //base tools for the chat session. - is static strings the way to go here? no contract enforcement.
        private HashSet<string> defaultChatTools = new HashSet<string>
        {
            KnownInformationSearch.ToolName,
            ImageEvaluate.ToolName,
            SendEmail.ToolName,
            NewsSearch.ToolName
        };


        public ToolDefinitionManager(IEnumerable<IToolDefinition> definedTools)
        {
            tools = definedTools.ToList();
        }

        public OpenAITool[] GetDefaultToolDefinitions()
        {
            return tools.Where(t => defaultChatTools.Contains(t.Name)).Select(t => t.GetToolRequestDefinition()).ToArray();
        }

        public void AddDefaultTool(IToolDefinition tool)
        {
            tools.Add(tool);
            defaultChatTools.Add(tool.Name);
        }

        public void RemoveDefaultTool(string toolName)
        {
            defaultChatTools.Remove(toolName);
            tools.RemoveAll(t => t.Name == toolName);
        }
    }
}
