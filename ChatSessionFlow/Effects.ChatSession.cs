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
            new FlowEffect<OpenAIChatRequest>(OnChatRequested_CallChatGPT_ResolveResponseReceived, ChatSessionActions.ChatRequested()),
            //new FlowEffect(OnResponseValidationRequested_CallChatGPT_ResolveResponseReceived, ChatSessionActions.ResponseValidatonRequested())
        };

        //effect methods
        public FlowActionBase OnInitialMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
        {

            List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
            newContext.Add(new OpenAISystemMessage("You are a helpful AI assistant. Consider the steps involved in resolving the prompt and if the tools need to be run in order. " +
                "\nIf a tool doesn't work, consider whether or not you can provide the content yourself."));
            //TODO: get session context eventually to populate newcontext & potentially model
            newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
            OpenAIChatRequest chatRequest = new OpenAIChatRequest
            {
                model = "gpt-3.5-turbo", //TODO: make these a const or something - magic strings bad.
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

        //public FlowActionBase OnResponseValidationRequested_CallChatGPT_ResolveResponseReceived(FlowActionBase flowAction)
        //{
        //    var currentContext = _flowStateData.CurrentState(ChatSessionSelectors.GetChatContext);
        //    List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
        //    newContext.Add(new OpenAIUserMessage("Agressively audit your response. Did you successfully, fully, completely resolve the original prompt?\n " +
        //        "IF YOU FAILED - PLEASE TRY AGAIN AND SKIP TOOL CALLS THAT DIDN'T WORK"));
        //    //TODO: get session context eventually to populate newcontext & potentially model
        //    //newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
        //    OpenAIChatRequest chatRequest = new OpenAIChatRequest
        //    {
        //        model = "gpt-3.5-turbo",
        //        //model = "gpt-4o",
        //        messages = newContext,
        //        temperature = 1,
        //        tools = _toolManager.GetToolDefinitions()
        //    };
        //    return ChatSessionActions.ChatRequested(chatRequest);
        //}
    }
}
