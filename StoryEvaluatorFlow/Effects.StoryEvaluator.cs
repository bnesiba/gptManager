using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.ChatGPTRepository;
using ToolManagement;
using ActionFlow.Models;
using ActionFlow;
using ChatSessionFlow.models;
using StoryEvaluatorFlow;
using StoryEvaluatorFlow.Models;
using ToolManagement.ToolDefinitions.Models;
using ToolManagement.ToolDefinitions.StoryEvaluatorTools;
using FakeDataStorageManager;

namespace ChatSessionFlow
{
    public class StoryEvaluatorEffects : IFlowStateEffects
    {
        private ToolDefinitionManager _toolManager;
        private FlowStateData<StoryEvaluatorEntity> _storyEvaluatorState;
        private FlowActionHandler _flowActionHandler;
        private TotallyRealDatabase<StoryEvaluatorEntity> _totallyADatabase;



        public StoryEvaluatorEffects(FlowActionHandler flowHandler, ToolDefinitionManager toolManager, FlowStateData<StoryEvaluatorEntity> storyState, TotallyRealDatabase<StoryEvaluatorEntity> storyDatabase)
        {
            _toolManager = toolManager;
            _flowActionHandler = flowHandler;
            _storyEvaluatorState = storyState;
            _totallyADatabase = storyDatabase;

        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnInitialStoryMsg_CreateChatRequest_ResolveChatRequestedAndStoryEvalComplete, StoryEvaluatorActions.InitStoryEval()),
           this.effect(OnToolExecutionRequested_WhenInitiatingNewEvaluation_ResolveInitStoryEval, ChatSessionActions.ToolExecutionRequested()),
           this.effect(OnInitialStoryChat_CreateChatRequest_ResolveChatRequested,StoryEvaluatorActions.InitStoryChat())
        };

        //Effect Methods

        public FlowActionBase OnInitialStoryMsg_CreateChatRequest_ResolveChatRequestedAndStoryEvalComplete(FlowAction<InitialMessage> initialMsg)
        {

            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            newContext.Add(new OpenAISystemMessage("You are a story tracker. You evaluate stories and extract/provide information about the story such as authors, characters, content-tags, and themes"));
            newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
            _toolManager.UseStoryEvaluatorTools();
            OpenAIChatRequest chatRequest = new OpenAIChatRequest
            {
                model = "gpt-4o-mini", //TODO: make these a const or something - magic strings bad.
                //model = "gpt-4o",
                messages = newContext,
                temperature = 1,
                tools = _toolManager.GetDefaultToolDefinitions(),
                //tool_choice = _toolManager.GetDefaultToolDefinitions().First() //If this is commented out, both tools still seem to get used correctly.
            };

            _flowActionHandler.ResolveAction(ChatSessionActions.ChatRequested(chatRequest));

            var storyData = _storyEvaluatorState.CurrentState(StoryEvaluatorSelectors.GetStoryEvaluation);
            _totallyADatabase.AddStory(storyData, initialMsg.Parameters.message);

            return StoryEvaluatorActions.StoryEvalCompleted();
        }

        public FlowActionBase OnToolExecutionRequested_WhenInitiatingNewEvaluation_ResolveInitStoryEval(
            FlowAction<ToolRequestParameters> toolRequest)
        {
            if (toolRequest.Parameters.ToolName == EvaluateNewStory.ToolName)
            {
                var storyToEvaluate = toolRequest.Parameters.GetStringParam("StoryToEvaluate") ?? string.Empty; //TODO: Get parameter strings from the tools somehow
                return StoryEvaluatorActions.InitStoryEval(storyToEvaluate);
            }
            return StoryEvaluatorActions.StoryEvalNoop("StoryEvaluatorEffects-OnToolExecutionRequested_WhenInitiatingNewEvaluation_ResolveInitStoryEval");
        }

        public FlowActionBase OnInitialStoryChat_CreateChatRequest_ResolveChatRequested(
            FlowAction<InitialMessage> initialMessage)
        {
            //            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            //newContext.Add(new OpenAISystemMessage("You are a helpful AI assistant. Consider the steps involved in resolving the prompt and if the tools need to be run in order. \" +\r\n\"\\nIf a tool doesn't work, consider whether or not you can provide the content yourself.\""));
            //newContext.Add(new OpenAIUserMessage(initialMessage.Parameters.message));
            _toolManager.UseStoryChatTools();
            //OpenAIChatRequest chatRequest = new OpenAIChatRequest
            //{
            //    model = "gpt-4o-mini", //TODO: make these a const or something - magic strings bad.
            //    //model = "gpt-4o",
            //    messages = newContext,
            //    temperature = 1,
            //    tools = _toolManager.GetDefaultToolDefinitions(),
            //};

            //return ChatSessionActions.ChatRequested(chatRequest);
            return ChatSessionActions.InitAssistantChat(initialMessage.Parameters.message, initialMessage.Parameters.sessionId);
        
        }

    }
}
