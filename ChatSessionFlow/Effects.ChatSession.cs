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
            new FlowEffect<InitialMessage>(OnInitialMsg_CreateChatRequest_ResolveChatRequested, ChatSessionActions.Init()),
            new FlowEffect<OpenAIChatRequest>(OnChatRequested_CallChatGPT_ResolveResponseReceived, ChatSessionActions.ChatRequested())
        };

        //effect methods
        public FlowActionBase OnInitialMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
        {

            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            newContext.Add(new OpenAISystemMessage("You are a tool-using AI assistant. Consider how to resolve the user's request and the use tools as needed. " +
                "\nIf you need to run multiple tools and one isn't working, move on and do your best."));
            //TODO: get session context eventually to populate newcontext & potentially model
            newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
            OpenAIChatRequest chatRequest = new OpenAIChatRequest
            {
                model = "gpt-3.5-turbo",
                //model = "gpt-4o",
                messages = newContext,
                temperature = 1,
                tools = _toolManager.GetToolDefinitions()
            };
            return ChatSessionActions.ChatRequested(chatRequest);
        }

        public FlowActionBase OnChatRequested_CallChatGPT_ResolveResponseReceived(FlowAction<OpenAIChatRequest> chatRequest)
        {

            var response = _chatGPTRepo.Chat(chatRequest.Parameters);
            return ChatSessionActions.ChatResponseReceived(response);
        }
    }
}
