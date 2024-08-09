using OpenAIConnector.ChatGPTRepository.models;
using System.Linq;

namespace ChatSessionFlow.models
{
    public class ChatSessionEntity
    {
        public List<OpenAIChatMessage> CurrentContext { get; set; }

        public Stack<OpenAIChatRequest> ChatRequestStack { get; set; }

        //TODO: remove me - testing param
        public int NumberOfChats { get; set; }

        public ChatSessionEntity()
        {
            CurrentContext = new List<OpenAIChatMessage>();
            ChatRequestStack = new Stack<OpenAIChatRequest>();
            NumberOfChats = 0;
        }

        public ChatSessionEntity Copy()
        {
            var copy = new ChatSessionEntity();
            copy.CurrentContext = CurrentContext;
            copy.ChatRequestStack = new Stack<OpenAIChatRequest>();
            copy.NumberOfChats = NumberOfChats;
            ChatRequestStack.ToList().ForEach(r => copy.ChatRequestStack.Push(r.Copy()));
            return copy;
        }

        public override string ToString()
        {
            return $"ContextLength: {CurrentContext.Count} LinksLen: {ChatRequestStack.Count} NumberOfChats: {NumberOfChats}";
        }
    }
}
