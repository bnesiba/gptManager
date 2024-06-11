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
            this.reduce(CurrentContext_WhenInitialMsgReceived_AddToContext,ChatSessionActions.Init()),
            this.reduce(CurrentContext_WhenChatResponseReceived_AddToContext, ChatSessionActions.ChatResponseReceived()),
            this.reduce(CurrentContext_ToolExecutionCompleted_AddToContext, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(CurrentContext_ChatRequestReceived_IncrementCount, ChatSessionActions.ChatRequested())
        };


        //Reducer Methods
        public ChatSessionEntity CurrentContext_WhenInitialMsgReceived_AddToContext(FlowAction<InitialMessage> initAction,ChatSessionEntity currentState)
        {
            currentState.CurrentContext.Add(new OpenAIUserMessage(initAction.Parameters.message));
            return currentState;
        }

        public ChatSessionEntity CurrentContext_WhenChatResponseReceived_AddToContext(FlowAction<OpenAIChatResponse> responseAction,ChatSessionEntity currentState)
        {
            currentState.CurrentContext.Add(responseAction.Parameters.choices[0].message);
            return currentState;
        }

        public ChatSessionEntity CurrentContext_ToolExecutionCompleted_AddToContext(FlowAction<OpenAIToolMessage> toolExecutedAction, ChatSessionEntity currentState)
        {
            currentState.CurrentContext.Add(toolExecutedAction.Parameters);
            return currentState;
        }

        public ChatSessionEntity CurrentContext_ChatRequestReceived_IncrementCount(FlowAction<OpenAIChatRequest> chatRequestAction, ChatSessionEntity currentState)
        {
            currentState.NumberOfChats++;
            return currentState;
        }

    }
}
