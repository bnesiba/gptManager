using OpenAIConnector.ChatGPTRepository.models;
using System.Linq;

namespace ChatSessionFlow.models
{
    public class StoryEvalEntity
    {
        public List<StoryCharacter> StoryCharacters { get; set; }

        public string ShortSummary { get; set; }

        public HashSet<string> StoryTags { get; set; }

        public List<string> evalComments { get; set; }


        public StoryEvalEntity()
        {
            StoryCharacters = new List<StoryCharacter>();
            ShortSummary = "";
            StoryTags = new HashSet<string>();
            evalComments = new List<string>();
        }

        public StoryEvalEntity Copy()
        {
            var copy = new StoryEvalEntity();
            copy.StoryCharacters = StoryCharacters.ToList();
            copy.ShortSummary = ShortSummary;
            copy.StoryTags = StoryTags.ToHashSet();
            copy.evalComments = evalComments;
            return copy;
        }

        public override string ToString()
        {
            return $"CharacterCount: {StoryCharacters.Count} Tags: [{string.Join(',',StoryTags)}]";
        }
    }

    public class StoryCharacter
    {
        public string CharacterName { get; set; }
        public string CharacterDescription { get; set; }
        public string CharacterRole { get; set; }
    }

}
