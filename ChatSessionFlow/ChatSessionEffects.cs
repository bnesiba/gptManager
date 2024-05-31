using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.ChatGPTRepository;
using SessionStateFlow.package.Models;
using SessionStateFlow.package;
using ChatSessionFlow.models;
using ToolManagement;

namespace ChatSessionFlow
{
    public class ChatSessionEffects : IFlowStateEffects
    {
        private ChatGPTRepo _chatGPTRepo;
        private FlowStateData<ChatSessionEntity> _flowStateData;
        private ToolDefinitionManager _toolManager;
        

        public ChatSessionEffects(FlowStateData<ChatSessionEntity> stateData, ToolDefinitionManager toolManager, ChatGPTRepo chatRepo)
        {
            _flowStateData = stateData;
            _chatGPTRepo = chatRepo;
            _toolManager = toolManager;
        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
            new FlowEffect<InitialMessage>(OnInitialMsg_CreateChatRequest_ResolveChatRequested, ChatSessionActions.init()),
            new FlowEffect<OpenAIChatRequest>(OnChatRequested_CallChatGPT_ResolveResponseReceived, ChatSessionActions.chatRequested())
        };

        public FlowActionBase OnInitialMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
        {

            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            //TODO: get session context eventually to populate newcontext & potentially model
            newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
            OpenAIChatRequest chatRequest = new OpenAIChatRequest
            {
                model = "gpt-3.5-turbo",
                messages = newContext,
                temperature = 1,
                tools = _toolManager.GetToolDefinitions()
            };
            return ChatSessionActions.chatRequested(chatRequest);
        }

        public FlowActionBase OnChatRequested_CallChatGPT_ResolveResponseReceived(FlowAction<OpenAIChatRequest> chatRequest)
        {
            //TODO: remove me
            var count = _flowStateData.CurrentState(ChatSessionSelectors.GetChatCount);
            System.Diagnostics.Debug.WriteLine("ChatCount: ", count);//TODO: removeme

            var response = _chatGPTRepo.Chat(chatRequest.Parameters);
            return ChatSessionActions.chatResponseReceived(response);
        }
    }
}
