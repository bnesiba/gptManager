using ActionFlow;
using ActionFlow.Models;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace StoryEvaluatorFlow.Tools
{
    public class EvaluateNewStory : IToolDefinition
    {
        //private FlowState _flowState;

        //static accessor for Tool Management
        public static string ToolName => "EvaluateNewStory";

        public string Name => ToolName;

        public string Description => "Run the evaluation process for a new story, causing it to be stored and searchable";

        public EvaluateNewStory(/*FlowState flowstate*/)
        {
            //_flowState = flowstate;
        }

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "StoryToEvaluate",
                type = "string",
                description = "The story to run the evaluation process for",
                IsRequired = true
            },

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {
            var storyToEvaluate = toolParams.GetStringParam("StoryToEvaluate");
            //_flowState.ResolveAction(InitStoryEval(storyToEvaluate)); //TODO: figure out circular dependency issues preventing import of FlowState so action impl doesnt have to be in effects. Maybe move actions out somewhere?
            return new OpenAIToolMessage($"EvaluateNewStory: " + "Evaluation Executed!", toolParams.ToolRequestId);
        }

    }
}