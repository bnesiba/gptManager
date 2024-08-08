using ActionFlow.Models;
using ChatSessionFlow.models;

namespace StoryEvaluatorFlow
{
    public static class StoryEvaluatorActions
    {
        public static FlowAction<InitialMessage> InitStoryEval(string story = "", Guid? chatSessionId = null) => new FlowAction<InitialMessage> { Name = "InitialStoryMessage", Parameters = new InitialMessage { message = story, sessionId = chatSessionId ?? Guid.NewGuid() } };

    }
}
