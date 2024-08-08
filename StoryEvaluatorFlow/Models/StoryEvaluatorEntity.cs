
using FakeDataStorageManager;

namespace StoryEvaluatorFlow.Models
{
    public class StoryEvaluatorEntity: ITagSearchable
    {
        public string StoryTitle { get; set; }
        public List<string> Authors { get; set; }
        public List<StoryCharacter> StoryCharacters { get; set; }

        public string StorySummary { get; set; }

        public HashSet<string> SearchTags { get; set; }

        public List<string> evalComments { get; set; }


        public StoryEvaluatorEntity()
        {
            StoryTitle = "";
            Authors = new List<string>();
            StoryCharacters = new List<StoryCharacter>();
            StorySummary = "";
            SearchTags = new HashSet<string>();
            evalComments = new List<string>();
        }

        public StoryEvaluatorEntity Copy()
        {
            var copy = new StoryEvaluatorEntity();
            copy.StoryTitle = StoryTitle;
            copy.Authors = Authors.ToList();
            copy.StoryCharacters = StoryCharacters.ToList();
            copy.StorySummary = StorySummary;
            copy.SearchTags = SearchTags.ToHashSet();
            copy.evalComments = evalComments.ToList();
            return copy;
        }

        public override string ToString()
        {
            return $"Title: {StoryTitle} CharacterCount: {StoryCharacters.Count} Tags: [{string.Join(',',SearchTags)}]";
        }
    }

    public class StoryCharacter
    {
        public string CharacterName { get; set; }
        public string CharacterDescription { get; set; }
        public string CharacterRole { get; set; }
    }

}
