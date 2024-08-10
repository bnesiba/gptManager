using ActionFlow.Models;

namespace ToolManagementFlow
{
    public static class ToolManagementActions
    {
        public static FlowAction<List<string>> SetToolset(List<string>? toolNames = null) => new FlowAction<List<string>> { Name = "SetToolset", Parameters = toolNames ?? new List<string>() };
        public static FlowAction<string> AddToToolset(string? toolName = null) => new FlowAction<string> { Name = "AddToToolset", Parameters = toolName ?? string.Empty };
        public static FlowAction<string> RemoveFromToolset(string? toolName = null) => new FlowAction<string> { Name = "RemoveFrom", Parameters = toolName ?? string.Empty };
    }
}
