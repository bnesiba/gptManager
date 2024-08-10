
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace ChatSessionFlow.models
{
    public class CompletedToolResult
    {
        public string toolName { get; set; }
        public OpenAIToolMessage toolMessage { get; set; }
        public ToolRequestParameters toolRequestParameters { get; set; }
    }
}
