using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.ChatGPTRepository;
using ChatSessionFlow.models;
using ToolManagement;
using ActionFlow.Models;
using ActionFlow;
using ToolManagement.ToolDefinitions;

namespace ChatSessionFlow
{
    public class StoryEvalEffects : IFlowStateEffects
    {
        private ChatGPTRepo _chatGPTRepo;
        private FlowStateData<ChatSessionEntity> _flowStateData;
        private ToolDefinitionManager _toolManager;
        

        public StoryEvalEffects(FlowStateData<ChatSessionEntity> stateData, ToolDefinitionManager toolManager, ChatGPTRepo chatRepo)
        {
            _flowStateData = stateData;
            _chatGPTRepo = chatRepo;
            _toolManager = toolManager;
        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnInitialStoryMsg_CreateChatRequest_ResolveChatRequested, ChatSessionActions.InitStoryEval()),
        };

        //Effect Methods

        public FlowActionBase OnInitialStoryMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
        {

            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            newContext.Add(new OpenAISystemMessage("You are a story tracker. You evaluate stories and extract/provide information about the story such as authors,characters, relationships, and themes"));
            newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
            _toolManager.UseStoryExampleTools();
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
