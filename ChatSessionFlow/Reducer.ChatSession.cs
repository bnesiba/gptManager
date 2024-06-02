using OpenAIConnector.ChatGPTRepository.models;
using SessionStateFlow.package.Models;
using SessionStateFlow.package;
using SessionStateFlow;
using ChatSessionFlow.models;

namespace ChatSessionFlow
{
    //TODO: update to be more like effects?
    //Reducer
    public class ChatSessionReducer : IFlowStateReducer<ChatSessionEntity>
    {
        public ChatSessionEntity InitialState => new ChatSessionEntity();

        public ChatSessionEntity Reduce(FlowActionBase action, ChatSessionEntity currentState)
        {
            ChatSessionEntity newState = currentState;
            //manage context
            CurrentContext_WhenInitialMsgReceived_AddToContext(action, newState, out newState);
            CurrentContext_WhenResponseMsgReceived_AddToContext(action, newState, out newState);
            CurrentContext_ToolExecutionCompleted_AddToContext(action, newState, out newState);
            //manage count
            CurrentContext_ChatRequestReceived_IncrementCount(action, newState, out newState);
            CurrentContext_ChatResponseReceived_IncrementCount(action, newState, out newState);

            return newState;
        }


        public void CurrentContext_WhenInitialMsgReceived_AddToContext(FlowActionBase action, ChatSessionEntity currentState, out ChatSessionEntity newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ChatSessionActions.Init(), out var initAction))
            {
                newState.CurrentContext.Add(new OpenAIUserMessage(initAction.Parameters.message));
            }
        }

        public void CurrentContext_WhenResponseMsgReceived_AddToContext(FlowActionBase action, ChatSessionEntity currentState, out ChatSessionEntity newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ChatSessionActions.ChatResponseReceived(), out var responseMessage))
            {
                newState.CurrentContext.Add(responseMessage.Parameters.choices[0].message);
            }
        }
        public void CurrentContext_ToolExecutionCompleted_AddToContext(FlowActionBase action, ChatSessionEntity currentState, out ChatSessionEntity newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ChatSessionActions.ToolExecutionSucceeded(), out var toolAction))
            {
                newState.CurrentContext.Add(toolAction.Parameters);
            }
        }

        public void CurrentContext_ChatRequestReceived_IncrementCount(FlowActionBase action, ChatSessionEntity currentState, out ChatSessionEntity newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ChatSessionActions.ChatRequested(), out var _))
            {

                newState.NumberOfChats++;
            }
        }

        public void CurrentContext_ChatResponseReceived_IncrementCount(FlowActionBase action, ChatSessionEntity currentState, out ChatSessionEntity newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ChatSessionActions.ChatResponseReceived(), out var _))
            {

                newState.NumberOfChats++;
            }
        }

    }
}
