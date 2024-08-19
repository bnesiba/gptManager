using ActionFlow.Models;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace ToolManagementFlow
{
    public static class ToolManagementSelectors
    {
        public static FlowDataSelector<ToolManagementStateEntity, OpenAITool[]> GetToolset = new FlowDataSelector<ToolManagementStateEntity, OpenAITool[]>
            ((stateData) => stateData.tools.Where(t => stateData.CurrentTools.Contains(t.Name)).Select(t => t.GetToolRequestDefinition()).ToArray());

    }
}
