using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement.ToolDefinitions.StoryEvaluatorTools
{
    public class SetCharacterList : IToolDefinition
    {

        //static accessor for Tool Management
        public static string ToolName => "SetCharacterList";

        public string Name => ToolName;

        public string Description => "Set up the cast of characters from the current story - Make sure to include all of the characters in the story.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ArrayToolProperty()
            {
                name = "CharacterArray",
                type = "array",
                items = new ObjectToolProperty()
                {
                    type = "object",
                    properties = new List<ToolProperty>()
                    {
                        new ToolProperty()
                        {
                            name = "CharacterName",
                            type = "string",
                            description = "The name of the character",
                            IsRequired = true//TODO: don't let user set this anymore - per api spec if the array is required, each part of the array object *must* also be required
                        },
                        new ToolProperty()
                        {
                            name = "CharacterDescription",
                            type = "string",
                            description = "A brief description of the character, if the character appearance can be deduced",
                            IsRequired = true
                        },
                        new EnumToolProperty()
                        {
                            name = "CharacterRole",
                            type = "string",
                            description = "Whether or not the character is a protagonist, antagonist, or something else",
                            enumValues = new List<string>{"protagonist", "antagonist", "other"},
                            IsRequired = true
                        }
                    }
                },
                description = "An Array of character objects containing the name, description, and role of every character in the story. Make sure to include even minor characters, but don't make up any characters.",
                IsRequired = true
            },
            new ToolProperty()
            {
                name = "AdditionalInformation",
                type = "string",
                description = "Were there any complications listing all of the characters? what were they?",
                IsRequired = true
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            return new OpenAIToolMessage($"SetCharacterListResponse: " + "Thanks!", toolParams.ToolRequestId);
        }
    }
}