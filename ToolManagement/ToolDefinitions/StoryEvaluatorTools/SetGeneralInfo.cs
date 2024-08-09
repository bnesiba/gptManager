using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement.ToolDefinitions.StoryEvaluatorTools
{
    public class SetGeneralInfo : IToolDefinition
    {

        //static accessor for Tool Management
        public static string ToolName => "SetGeneralInfo";

        public string Name => ToolName;

        public string Description => "Set initial information about the story - Title and Author(s)";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "StoryTitle",
                type = "string",
                description = "What is the title of this story?",
                IsRequired = true
            },
            new ArrayToolProperty()
            {
                name = "AuthorArray",
                type = "array",
                items = new()
                {
                    type = "string",
                },
                description = "An Array of strings containing the Author(s) of the story",
                IsRequired = true
            },
            new ToolProperty()
            {
                name = "AdditionalInformation",
                type = "string",
                description = "Were there any complications finding the General Info? what were they?",
                IsRequired = true
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            return new OpenAIToolMessage($"SetGeneralInfo: " + "Thanks!", toolParams.ToolRequestId);
        }
    }
}