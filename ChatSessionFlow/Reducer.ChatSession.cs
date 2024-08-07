using OpenAIConnector.ChatGPTRepository.models;
using ChatSessionFlow.models;
using ActionFlow.Models;

namespace ChatSessionFlow
{
    //Reducer
    public class ChatSessionReducer : IFlowStateReducer<ChatSessionEntity>
    {
        public ChatSessionEntity InitialState => new ChatSessionEntity();

        public List<IFlowReductionBase<ChatSessionEntity>> Reductions => new List<IFlowReductionBase<ChatSessionEntity>>
        {
            this.reduce(CurrentContext_OnInitialMsgReceived_AddToContext,ChatSessionActions.InitAssistantChat()),
            this.reduce(CurrentContext_OnChatResponseReceived_AddToContext, ChatSessionActions.ChatResponseReceived()),
            this.reduce(CurrentContext_OnToolExecutionCompleted_AddToContext, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(RequestStack_OnChatRequested_PushRequestToStack, ChatSessionActions.ChatRequested()),
            this.reduce(RequestStack_OnChatRequestReceived_PopRequestFromStack, ChatSessionActions.ToolExecutionsCompleted(), ChatSessionActions.ToolsExecutionEmpty()),
            this.reduce(CurrentContext_OnChatRequestReceived_IncrementCount, ChatSessionActions.ChatRequested())
        };


        //Reducer Methods
        public ChatSessionEntity CurrentContext_OnInitialMsgReceived_AddToContext(FlowAction<InitialMessage> initAction,ChatSessionEntity currentState)
        {
            currentState.CurrentContext.Add(new OpenAIUserMessage(initAction.Parameters.message));
            return currentState;
        }

        public ChatSessionEntity CurrentContext_OnChatResponseReceived_AddToContext(FlowAction<OpenAIChatResponse> responseAction,ChatSessionEntity currentState)
        {
            if (responseAction.Parameters?.choices != null && responseAction.Parameters.choices.Count > 0)
            {
                currentState.CurrentContext.Add(responseAction.Parameters.choices[0].message);
            }
            return currentState;
        }

        public ChatSessionEntity CurrentContext_OnToolExecutionCompleted_AddToContext(FlowAction<OpenAIToolMessage> toolExecutedAction, ChatSessionEntity currentState)
        {
            currentState.CurrentContext.Add(toolExecutedAction.Parameters);
            return currentState;
        }

        public ChatSessionEntity RequestStack_OnChatRequested_PushRequestToStack(FlowAction<OpenAIChatRequest> chatRequestedAction, ChatSessionEntity currentState)
        {
            currentState.ChatRequestStack.Push(chatRequestedAction.Parameters);
            return currentState;
        }

        public ChatSessionEntity RequestStack_OnChatRequestReceived_PopRequestFromStack(FlowAction<List<OpenAIToolCall>> chatRequestedAction, ChatSessionEntity currentState)
        {
            currentState.ChatRequestStack.Pop();
            return currentState;
        }

        public ChatSessionEntity CurrentContext_OnChatRequestReceived_IncrementCount(FlowAction<OpenAIChatRequest> chatRequestAction, ChatSessionEntity currentState)
        {
            currentState.NumberOfChats++;
            return currentState;
        }

    }
}
