using OpenAIConnector.ChatGPTRepository.models;
using System.Linq;

namespace ChatSessionFlow.models
{
    public class ChatSessionEntity
    {
        public List<OpenAIChatMessage> CurrentContext { get; set; }

        public Dictionary<Guid, List<OpenAIChatMessage>> SessionContexts { get; set; }
        public Guid CurrentSession {  get; set; }
        public Stack<Guid> ChatSessionStack { get; set; }

        public Stack<OpenAIChatRequest> ChatRequestStack { get; set; }

        //TODO: remove me - testing param
        public int NumberOfChats { get; set; }

        public ChatSessionEntity()
        {
            CurrentContext = new List<OpenAIChatMessage>();
            SessionContexts = new Dictionary<Guid, List<OpenAIChatMessage>>();
            CurrentSession = Guid.NewGuid();
            ChatSessionStack = new Stack<Guid>();
            ChatRequestStack = new Stack<OpenAIChatRequest>();
            NumberOfChats = 0;
        }

        public ChatSessionEntity Copy()
        {
            var copy = new ChatSessionEntity();
            copy.CurrentContext = CurrentContext;
            copy.SessionContexts = SessionContexts;
            copy.CurrentSession = CurrentSession;
            copy.ChatSessionStack = new Stack<Guid>();
            copy.ChatRequestStack = new Stack<OpenAIChatRequest>();
            copy.NumberOfChats = NumberOfChats;
            ChatRequestStack.ToList().ForEach(r => copy.ChatRequestStack.Push(r.Copy()));
            ChatSessionStack.ToList().ForEach(s => copy.ChatSessionStack.Push(s));
            return copy;
        }

        public override string ToString()
        {
            return $"ContextLength: {CurrentContext.Count} LinksLen: {ChatRequestStack.Count} NumberOfChats: {NumberOfChats}";
        }
    }
}
