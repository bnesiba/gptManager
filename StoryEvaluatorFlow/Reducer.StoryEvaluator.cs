using ActionFlow.Models;
using ChatSessionFlow;
using ChatSessionFlow.models;
using OpenAIConnector.ChatGPTRepository.models;
using StoryEvaluatorFlow.Models;
using ToolManagement.ToolDefinitions.StoryEvaluatorTools;

namespace StoryEvaluatorFlow
{
    //Reducer
    public class StoryEvaluationReducer : IFlowStateReducer<StoryEvaluatorEntity>
    {
        public StoryEvaluatorEntity InitialState => new StoryEvaluatorEntity();

        public List<IFlowReductionBase<StoryEvaluatorEntity>> Reductions => new List<IFlowReductionBase<StoryEvaluatorEntity>>
        {
            this.reduce(StoryCharacters_OnSetCharacterListSuccess_SetStoryCharacters, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(EvalComments_OnSetCharacterListSuccess_AddEvalComments, ChatSessionActions.ToolExecutionSucceeded(), ChatSessionActions.ToolExecutionFailed()),

            this.reduce(StoryTags_OnSetStoryTagsSuccess_SetStoryTags, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(EvalComments_OnSetStoryTagsSuccessOrFailure_AddEvalComments, ChatSessionActions.ToolExecutionSucceeded(), ChatSessionActions.ToolExecutionFailed()),

            this.reduce(StoryTitle_OnSetGeneralInfoSuccess_SetStoryTitle, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(StoryAuthors_OnSetGeneralInfoSuccess_SetStoryAuthors, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(EvalComments_OnSetGeneralInfoSuccessOrFailure_AddEvalComments, ChatSessionActions.ToolExecutionSucceeded(), ChatSessionActions.ToolExecutionFailed()),

            this.reduce(StorySummary_OnSetStorySummarySuccess_SetStorySummary, ChatSessionActions.ToolExecutionSucceeded()),
            this.reduce(EvalComments_OnSetStorySummarySuccessOrFailure_AddEvalComments, ChatSessionActions.ToolExecutionSucceeded(), ChatSessionActions.ToolExecutionFailed())
        };


        //Reducer Methods
        public StoryEvaluatorEntity StoryCharacters_OnSetCharacterListSuccess_SetStoryCharacters(FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetCharacterList.ToolName)
            {
                var setCharacters = 
                    toolResult.Parameters.toolRequestParameters.GetObjectArrayParam<StoryCharacter>("CharacterArray"); //TODO: constants...or maybe extensions on the tool?

                if (setCharacters != null)
                {
                    currentState.StoryCharacters = setCharacters;
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity EvalComments_OnSetCharacterListSuccess_AddEvalComments(FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetCharacterList.ToolName)
            {
                var evaluationComment =
                    toolResult.Parameters.toolRequestParameters.GetStringParam("AdditionalInformation"); //TODO: constants...or maybe extensions on the tool?

                if (evaluationComment != null)
                {
                    currentState.evalComments.Add($"{toolResult.Name} - {toolResult.Parameters.toolName}: {evaluationComment}");
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity StoryTags_OnSetStoryTagsSuccess_SetStoryTags(FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetStoryTags.ToolName)
            {
                var animalTags =
                    toolResult.Parameters.toolRequestParameters.GetStringArrayParam("Animals") ?? new List<string>(); //TODO: constants...or maybe extensions on the tool?

                var vehicleTags =
                    toolResult.Parameters.toolRequestParameters.GetStringArrayParam("Vehicles") ?? new List<string>(); 

                var readingLevelTag = 
                    toolResult.Parameters.toolRequestParameters.GetStringParam("ReadingLevel"); 

                bool containsMonsters =
                    toolResult.Parameters.toolRequestParameters.GetStringParam("Monsters") == "true";

                if (animalTags.Count > 0)
                {
                    animalTags.ForEach(tag => currentState.SearchTags.Add(tag));
                }

                if (vehicleTags.Count > 0)
                {
                    vehicleTags.ForEach(tag => currentState.SearchTags.Add(tag));
                }

                if (readingLevelTag != null)
                {
                    currentState.SearchTags.Add(readingLevelTag);
                }

                if (containsMonsters)
                {
                    currentState.SearchTags.Add("ContainsMonsters");
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity EvalComments_OnSetStoryTagsSuccessOrFailure_AddEvalComments(FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetStoryTags.ToolName)
            {
                var evaluationComment =
                    toolResult.Parameters.toolRequestParameters.GetStringParam("AdditionalInformation"); //TODO: constants...or maybe extensions on the tool?

                if (evaluationComment != null)
                {
                    currentState.evalComments.Add($"{toolResult.Name} - {toolResult.Parameters.toolName}: {evaluationComment}");
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity StoryTitle_OnSetGeneralInfoSuccess_SetStoryTitle(
            FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetGeneralInfo.ToolName)
            {
                var title = 
                    toolResult.Parameters.toolRequestParameters.GetStringParam("StoryTitle"); //TODO: constants...or maybe extensions on the tool?

                if (title != null)
                {
                    currentState.StoryTitle = title;
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity StoryAuthors_OnSetGeneralInfoSuccess_SetStoryAuthors(
            FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetGeneralInfo.ToolName)
            {
                var authors = 
                    toolResult.Parameters.toolRequestParameters.GetStringArrayParam("AuthorArray") ?? new List<string>(); //TODO: constants...or maybe extensions on the tool?

                if (authors.Count > 0)
                {
                    authors.ForEach(author => currentState.Authors.Add(author));
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity EvalComments_OnSetGeneralInfoSuccessOrFailure_AddEvalComments(
            FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetGeneralInfo.ToolName)
            {
                var evaluationComment =
                    toolResult.Parameters.toolRequestParameters.GetStringParam("AdditionalInformation"); //TODO: constants...or maybe extensions on the tool?

                if (evaluationComment != null)
                {
                    currentState.evalComments.Add($"{toolResult.Name} - {toolResult.Parameters.toolName}: {evaluationComment}");
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity StorySummary_OnSetStorySummarySuccess_SetStorySummary(
            FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetStorySummary.ToolName)
            {
                var summary = 
                    toolResult.Parameters.toolRequestParameters.GetStringParam("StorySummary"); //TODO: constants...or maybe extensions on the tool?

                if (summary != null)
                {
                    currentState.StorySummary = summary;
                }
            }
            return currentState;
        }

        public StoryEvaluatorEntity EvalComments_OnSetStorySummarySuccessOrFailure_AddEvalComments(
            FlowAction<CompletedToolResult> toolResult,
            StoryEvaluatorEntity currentState)
        {
            if (toolResult.Parameters.toolName == SetStorySummary.ToolName)
            {
                var evaluationComment =
                    toolResult.Parameters.toolRequestParameters.GetStringParam("AdditionalInformation"); //TODO: constants...or maybe extensions on the tool?

                if (evaluationComment != null)
                {
                    currentState.evalComments.Add($"{toolResult.Name} - {toolResult.Parameters.toolName}: {evaluationComment}");
                }
            }
            return currentState;
        }

    }
}
