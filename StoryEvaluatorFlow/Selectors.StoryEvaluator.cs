using ActionFlow.Models;
using ChatSessionFlow.models;
using OpenAIConnector.ChatGPTRepository.models;
using StoryEvaluatorFlow.Models;

namespace StoryEvaluatorFlow
{
    public static class StoryEvaluatorSelectors
    {
        public static FlowDataSelector<StoryEvaluatorEntity, StoryEvaluatorEntity> GetStoryEvaluation = new FlowDataSelector<StoryEvaluatorEntity, StoryEvaluatorEntity>((stateData) => stateData);

        public static FlowDataSelector<StoryEvaluatorEntity, int> GetCharacterCount = new FlowDataSelector<StoryEvaluatorEntity, int>((stateData) => stateData.StoryCharacters.Count);

        public static FlowDataSelector<StoryEvaluatorEntity, List<StoryCharacter>> GetStoryCharacters = new FlowDataSelector<StoryEvaluatorEntity, List<StoryCharacter>>((stateData) => stateData.StoryCharacters.ToList());

        public static FlowDataSelector<StoryEvaluatorEntity, List<string>> GetCurrentStoryTags = new FlowDataSelector<StoryEvaluatorEntity, List<string>>((stateData) => stateData.SearchTags.ToList());

        public static FlowDataSelector<StoryEvaluatorEntity, string> GetShortSummmary = new FlowDataSelector<StoryEvaluatorEntity, string>((stateData) => stateData.StorySummary);

    }
}
