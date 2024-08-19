using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace StoryEvaluatorFlow.Tools
{
    public class SetStorySummary : IToolDefinition
    {

        //static accessor for Tool Management
        public static string ToolName => "SetStorySummary";

        public string Name => ToolName;

        public string Description => "RUN THIS TOOL LAST - Set the StorySummary, which, along with other information, will be used to search for this and related stories.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "StorySummary",
                type = "string",
                description = "A short, Concise, one paragraph (or less) summary of the story",
                IsRequired = true
            },
            new ToolProperty()
            {
                name = "AdditionalInformation",
                type = "string",
                description = "Were there any complications generating the StorySummary? what were they?",
                IsRequired = true
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            return new OpenAIToolMessage($"SetGeneralInfo: " + "Thanks!", toolParams.ToolRequestId);
        }
    }
}