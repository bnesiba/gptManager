
namespace StoryEvaluatorFlow.Models
{
    public class StoryEvaluatorEntity
    {
        public string StoryTitle { get; set; }
        public List<string> StoryAuthors { get; set; }
        public List<StoryCharacter> StoryCharacters { get; set; }

        public string StorySummary { get; set; }

        public HashSet<string> StoryTags { get; set; }

        public List<string> evalComments { get; set; }


        public StoryEvaluatorEntity()
        {
            StoryTitle = "";
            StoryAuthors = new List<string>();
            StoryCharacters = new List<StoryCharacter>();
            StorySummary = "";
            StoryTags = new HashSet<string>();
            evalComments = new List<string>();
        }

        public StoryEvaluatorEntity Copy()
        {
            var copy = new StoryEvaluatorEntity();
            copy.StoryTitle = StoryTitle;
            copy.StoryAuthors = StoryAuthors.ToList();
            copy.StoryCharacters = StoryCharacters.ToList();
            copy.StorySummary = StorySummary;
            copy.StoryTags = StoryTags.ToHashSet();
            copy.evalComments = evalComments.ToList();
            return copy;
        }

        public override string ToString()
        {
            return $"Title: {StoryTitle} CharacterCount: {StoryCharacters.Count} Tags: [{string.Join(',',StoryTags)}]";
        }
    }

    public class StoryCharacter
    {
        public string CharacterName { get; set; }
        public string CharacterDescription { get; set; }
        public string CharacterRole { get; set; }
    }

}
