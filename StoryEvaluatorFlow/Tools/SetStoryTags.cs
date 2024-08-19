using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace StoryEvaluatorFlow.Tools
{
    public class SetStoryTags : IToolDefinition
    {

        //static accessor for Tool Management
        public static string ToolName => "SetStoryTags";
        //TODO: ↓↓↓ make these into constants? also centralize them 
        public static List<string> animalTags = new List<string> { "dog", "cat", "bird", "pig", "mongoose", "snake", "other-animal" };
        public static List<string> vechicleTags = new List<string> { "car", "truck", "boat", "plane","train", "bike", "other-vehicle" };
        public static List<string> peopleTags = new List<string> { "boy", "girl", "mother", "father", "king", "queen", "farmer", "somethingelse-person" };
        public static List<string> readingLevelTags = new List<string> { "appropriate-for-newborns", "appropriate-for-toddler", "appropriate-for-older-child", "appropriate-for-young-adult", "appropriate-for-adult", "appropriate-for-old-person" };

        public string Name => ToolName;

        public string Description => "Add various types of tags to the story. These tags identify objects of interest in the story and are used to relate and filter different stories.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ArrayToolProperty()
            {
                name = "AnimalsInTheStory",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "tag representing a type of animal in the provided story - generalize if you have to (owls are birds, etc.)",
                    enumValues = animalTags,
                    IsRequired = true
                },
                description = "An array of strings representing types of animals that were characters in the provided story. Only include values provided by the enum - other values are \"other-animal\"",
                IsRequired = true
            },
            new ArrayToolProperty()
            {
                name = "VehiclesInTheStory",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "vehicles in the story",
                    enumValues = vechicleTags,
                    IsRequired = true
                },
                description = "An array of strings representing types of vehicles that were in the provided story. Only include values provided by the enum - other values are \"other-vehicle\"",
                IsRequired = true
            },
            new ArrayToolProperty()
            {
                name = "PeopleInTheStory",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "kinds of HUMAN people in the story",
                    enumValues = peopleTags,
                    IsRequired = true
                },
                description = "An array of strings representing types of HUMAN people in the provided story (empty if no humans in the story)",
                IsRequired = true
            },
            new EnumToolProperty()
            {
                name = "ContainsMonster",
                type = "string",
                description = "Does this story include a fictional monster?",
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