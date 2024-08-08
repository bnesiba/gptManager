using FakeDataStorageManager;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement.ToolDefinitions.StoryEvaluatorTools
{
    public class SearchForStories : IToolDefinition
    {
        private ITotallyADatabase _definitelyADatabase; 

        
        //static accessor for Tool Management
        public static string ToolName => "SearchForStories";

        public string Name => ToolName;

        public string Description => "Search for stories by author OR by pre-processed tags describing thing like animals and vehicles in the story or appropriate age-group. The results include all stories that match at least one of the search terms";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ArrayToolProperty()
            {
                name = "SearchTags",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "A story tag you want to find stories tagged with",
                    enumValues = SetStoryTags.animalTags.Concat(SetStoryTags.vechicleTags).Concat(SetStoryTags.readingLevelTags).ToList(),
                    IsRequired = true
                },
                description = "An array of strings representing tags to search for stories by",
                IsRequired = true
            },

            new ArrayToolProperty()
            {
                name = "SearchAuthors",
                type = "array",
                items =  new ToolProperty()
                {
                    type = "string",
                    description = "An author you want to find stories by",
                    IsRequired = true
                },
                description = "An array of strings representing authors to search for stories by",
                IsRequired = true
            }

        };

        public SearchForStories(ITotallyADatabase aDatabase)
        {
            _definitelyADatabase = aDatabase;
        }

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {
            var searchTags = toolParams.GetStringArrayParam("SearchTags") ?? new List<string>();
            var searchAuthors = toolParams.GetStringArrayParam("SearchAuthors") ?? new List<string>();
            var results = _definitelyADatabase.GetSerializedResultsByTagsAndAuthors(searchTags, searchAuthors);

            return new OpenAIToolMessage($"SearchForStories: " + string.Join("\r\n",results), toolParams.ToolRequestId);
        }
    }
}