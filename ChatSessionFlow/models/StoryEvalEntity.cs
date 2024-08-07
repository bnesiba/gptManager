using OpenAIConnector.ChatGPTRepository.models;
using System.Linq;

namespace ChatSessionFlow.models
{
    public class StoryEvalEntity
    {
        public List<StoryCharacter> StoryCharacters { get; set; }

        public string ShortSummary { get; set; }

        public List<string> StoryTags { get; set; }

        public List<string> evalComments { get; set; }


        public StoryEvalEntity()
        {
            StoryCharacters = new List<StoryCharacter>();
            ShortSummary = "";
            StoryTags = new List<string>();
            evalComments = new List<string>();
        }

        public StoryEvalEntity Copy()
        {
            var copy = new StoryEvalEntity();
            copy.StoryCharacters = StoryCharacters.ToList();
            copy.ShortSummary = ShortSummary;
            copy.StoryTags = StoryTags.ToList();
            copy.evalComments = evalComments;
            return copy;
        }

        public override string ToString()
        {
            return $"";
        }
    }

    public class StoryCharacter
    {
        public string CharacterName { get; set; }
        public string CharacterDescription { get; set; }
        public string CharacterRole { get; set; }
    }
    }
}
