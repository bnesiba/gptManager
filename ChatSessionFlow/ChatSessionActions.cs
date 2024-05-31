using ChatSessionFlow.models;
using OpenAIConnector.ChatGPTRepository.models;
using SessionStateFlow.package.Models;

namespace ChatSessionFlow
{
    public static class ChatSessionActions
    {
        public static FlowAction<InitialMessage> init(string initalMsg = "", Guid? chatSessionId = null) => new FlowAction<InitialMessage> { Name = "UserInitialMessage", Parameters = new InitialMessage { message = initalMsg, sessionId = chatSessionId ?? Guid.NewGuid() } };
        public static FlowAction<OpenAIChatRequest> chatRequested(OpenAIChatRequest? request = null) => new FlowAction<OpenAIChatRequest> { Name = "AIChatRequested", Parameters = request };
        public static FlowAction<OpenAIChatResponse> chatResponseReceived(OpenAIChatResponse? response = null) => new FlowAction<OpenAIChatResponse> { Name = "AIChatResponse", Parameters = response };
    }
}
