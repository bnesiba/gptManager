using OpenAIConnector.ChatGPTRepository.models;

namespace ChatSessionFlow.models
{
    public class ChatSessionEntity
    {
        public List<OpenAIChatMessage> CurrentContext { get; set; }

        public List<string> RelatedLinks { get; set; }

        //TODO: remove me - testing param
        public int NumberOfChats { get; set; }

        public ChatSessionEntity()
        {
            CurrentContext = new List<OpenAIChatMessage>();
            RelatedLinks = new List<string>();
            NumberOfChats = 0;
        }

        public ChatSessionEntity Copy()
        {
            var copy = new ChatSessionEntity();
            copy.CurrentContext = CurrentContext;
            copy.RelatedLinks = new List<string>();
            copy.NumberOfChats = NumberOfChats;
            RelatedLinks.ForEach(l => copy.RelatedLinks.Add(l));
            return copy;
        }

        public override string ToString()
        {
            return $"ContextLength: {CurrentContext.Count} LinksLen: {RelatedLinks.Count} NumberOfChats: {NumberOfChats}";
        }
    }
}
