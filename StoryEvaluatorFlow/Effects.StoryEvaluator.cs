using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.ChatGPTRepository;
using ToolManagement;
using ActionFlow.Models;
using ActionFlow;
using ChatSessionFlow.models;
using StoryEvaluatorFlow;

namespace ChatSessionFlow
{
    public class StoryEvaluatorEffects : IFlowStateEffects
    {
        private ToolDefinitionManager _toolManager;
        

        public StoryEvaluatorEffects(FlowStateData<ChatSessionEntity> stateData, ToolDefinitionManager toolManager, ChatGPTRepo chatRepo)
        {
            _toolManager = toolManager;
        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnInitialStoryMsg_CreateChatRequest_ResolveChatRequested, StoryEvaluatorActions.InitStoryEval()),
        };

        //Effect Methods

        public FlowActionBase OnInitialStoryMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
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
            return ChatSessionActions.ChatRequested(chatRequest);
        }

    }
}
