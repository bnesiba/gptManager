using ActionFlow.Models;
using ChatSessionFlow.models;

namespace StoryEvaluatorFlow
{
    public static class StoryEvaluatorActions
    {
        public static FlowAction<InitialMessage> InitStoryEval(string story = "", Guid? chatSessionId = null) => new FlowAction<InitialMessage> { Name = "InitialStoryMessage", Parameters = new InitialMessage { message = story, sessionId = chatSessionId ?? Guid.NewGuid() } };
        public static FlowAction StoryEvalCompleted() => new FlowAction { Name = "StoryEvalCompleted" };
        public static FlowAction<string> StoryEvalNoop(string source = "") => new FlowAction<string> { Name = "StoryEvalNoop", Parameters = source};

        public static FlowAction<InitialMessage> InitStoryChat(string searchMessage = "", Guid? chatSessionId = null) => new FlowAction<InitialMessage> { Name = "InitialStoryChat", Parameters = new InitialMessage { message = searchMessage, sessionId = chatSessionId ?? Guid.NewGuid() } };
    }
}
