using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions;
using ToolManagement.ToolDefinitions.Models;
using ToolManagement.ToolDefinitions.StoryEvalTools;

namespace ToolManagement
{
    //TODO: does this still need to exist? probably not here.
    //maybe if I end up actually having multiple sets of tools?
    public class ToolDefinitionManager
    {
        private List<IToolDefinition> tools = new List<IToolDefinition>();


        private HashSet<string> assistantChatTools = new HashSet<string>
        {
            KnownInformationSearch.ToolName,
            ImageEvaluate.ToolName,
            SendEmail.ToolName,
            NewsSearch.ToolName
        };

        private HashSet<string> storyProcTools = new HashSet<string>
        {
            SetCharacterList.ToolName,
            SetStoryTags.ToolName
        };


        private HashSet<string> defaultChatTools;


        public ToolDefinitionManager(IEnumerable<IToolDefinition> definedTools)
        {
            tools = definedTools.ToList();
            defaultChatTools = assistantChatTools;//default to assistant tools for now
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

        public void ClearDefaultTools()
        {
            defaultChatTools.Clear();
        }

        public void UseStoryExampleTools()
        {
            defaultChatTools = storyProcTools;
        }

        public void UseAssistantTools()
        {
            defaultChatTools = assistantChatTools;
        }
    }
}
