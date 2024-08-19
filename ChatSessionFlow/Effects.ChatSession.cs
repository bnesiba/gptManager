using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.ChatGPTRepository;
using ChatSessionFlow.models;
using ActionFlow.Models;
using ActionFlow;
using ToolManagementFlow.Models;
using ToolManagementFlow;
using ChatSessionFlow.ToolDefinitions;

namespace ChatSessionFlow
{
    public class ChatSessionEffects : IFlowStateEffects
    {
        private ChatGPTRepo _chatGPTRepo;
        private FlowActionHandler _flowActionHandler;
        private FlowStateData<ChatSessionEntity> _flowStateData;
        private FlowStateData<ToolManagementStateEntity> _toolStateData;
        

        public ChatSessionEffects(FlowActionHandler flowActHandler, FlowStateData<ChatSessionEntity> stateData, FlowStateData<ToolManagementStateEntity> toolData, ChatGPTRepo chatRepo)
        {
            _flowActionHandler = flowActHandler;
            _flowStateData = stateData;
            _toolStateData = toolData;
            _chatGPTRepo = chatRepo;
        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnInitialChatMsg_CreateChatRequest_ResolveChatRequested, ChatSessionActions.InitAssistantChat()),
           this.effect(OnTooledAsstantInit_SetToolset_ResolveInitAssistantChat, ChatSessionActions.InitTooledAssistantChat()),
           this.effect(OnChatRequested_CallChatGPT_ResolveResponseReceived, ChatSessionActions.ChatRequested()),
        };

        //Effect Methods
        public FlowActionBase OnInitialChatMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
        {
            var tools = _toolStateData.CurrentState(ToolManagementSelectors.GetToolset);
            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            newContext.Add(new OpenAISystemMessage("You are a helpful AI assistant. Consider the steps involved in resolving the prompt and if the tools need to be run in order. " +
                "\nIf a tool doesn't work, consider whether or not you can provide the content yourself."));
            //TODO: get session context eventually to populate newcontext & potentially model
            newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
            OpenAIChatRequest chatRequest = new OpenAIChatRequest
            {
                model = "gpt-4o-mini", //TODO: make these a const or something - magic strings bad.
                //model = "gpt-4o",
                messages = newContext,
                temperature = 1,
                tools = tools
            };
            return ChatSessionActions.ChatRequested(chatRequest);
        }

        public FlowActionBase OnTooledAsstantInit_SetToolset_ResolveInitAssistantChat(FlowAction<InitialMessage> initialMsgAction)
        {
            _flowActionHandler.ResolveAction(ToolManagementActions.SetToolset(new List<string>
            {
                KnownInformationSearch.ToolName,
                ImageEvaluate.ToolName,
                SendEmail.ToolName,
                NewsSearch.ToolName
            }));
            return ChatSessionActions.InitAssistantChat(initialMsgAction.Parameters.message, initialMsgAction.Parameters.sessionId);
        }

        public FlowActionBase OnChatRequested_CallChatGPT_ResolveResponseReceived(FlowAction<OpenAIChatRequest> chatRequest)
        {

            var response = _chatGPTRepo.Chat(chatRequest.Parameters);
            return ChatSessionActions.ChatResponseReceived(response);
        }
    }
}
