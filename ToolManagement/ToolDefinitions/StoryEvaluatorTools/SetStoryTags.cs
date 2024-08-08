using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement.ToolDefinitions.StoryEvaluatorTools
{
    public class SetStoryTags : IToolDefinition
    {

        //static accessor for Tool Management
        public static string ToolName => "SetStoryTags";

        public static List<string> animalTags = new List<string> { "dog", "cat", "songbird", "snake", "bug", "wolf", "pig", "cow", "goat", "chicken", "sheep", "other" };
        public static List<string> vechicleTags = new List<string> { "car", "truck", "boat", "plane", "bike", "other" };
        public static List<string> readingLevelTags = new List<string> { "newborn", "toddler", "older-child", "young-adult", "adult", "old-person" };

        public string Name => ToolName;

        public string Description => "Add various types of tags to the story. These tags identify themes and objects of interest in the story and are used to relate and filter different stories.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ArrayToolProperty()
            {
                name = "Animals",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "What kinds of animals were present in the story. Only pick \"other\" if there isn't a close option (for example: a puppy is a dog, a robin is a songbird) ",
                    enumValues = animalTags,
                    IsRequired = true
                },
                description = "An array of strings representing types of animals that were present in the story",
                IsRequired = true
            },
            new ArrayToolProperty()
            {
                name = "Vehicles",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "What kinds of vehicles were present in the story",
                    enumValues = vechicleTags,
                    IsRequired = true
                },
                description = "An array of strings representing types of vehicles that were present in the story",
                IsRequired = true
            },
            new EnumToolProperty()
            {
                name = "Monsters",
                type = "string",
                description = "Does this story include a monster?",
                enumValues = new List<string>{"true", "false"},
                IsRequired = true
            },
            new EnumToolProperty()
            {
                name = "ReadingLevel",
                type = "string",
                description = "What age-group is this story most appropriate for?",
                enumValues = readingLevelTags,
                IsRequired = true
            },
            new ToolProperty()
            {
                name = "AdditionalInformation",
                type = "string",
                description = "Were there any complications listing the story tags? what were they?",
                IsRequired = true
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            return new OpenAIToolMessage($"SetStoryTags: " + "Thanks!", toolParams.ToolRequestId);
        }
    }
}