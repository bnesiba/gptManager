using OpenAIConnector.ChatGPTRepository.models;
using SessionStateFlow.package;
using SessionStateFlow.package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow
{
    //TODO: This is an example. When finished flow should be in nuget package and these should be in separate files in the main project (probably).
    
    //TODO: find a way to unify actions
    public static class ProjectActions
    {
        public static FlowAction<string> init(string initalMsg = "") => new FlowAction<string> { Name = "UserInitialMessage", Parameters = initalMsg };
        public static FlowAction<OpenAIChatRequest> chatRequested(OpenAIChatRequest? request = null) => new FlowAction<OpenAIChatRequest> { Name = "AIChatRequested", Parameters = request };
        public static FlowAction<OpenAIChatResponse> chatResponseReceived(OpenAIChatResponse? response = null) => new FlowAction<OpenAIChatResponse> { Name = "AIChatResponse", Parameters = response };
    }

    public class ProjectStateModel
    {
        public List<OpenAIChatMessage> CurrentContext { get; set; }

        public List<string> RelatedLinks { get; set; }

        //TODO: remove me - testing param
        public  int NumberOfChats { get; set; }

        public ProjectStateModel()
        {
            CurrentContext = new List<OpenAIChatMessage>();
            RelatedLinks = new List<string>();
            NumberOfChats = 0;
        }

        public ProjectStateModel Copy()
        {
            var copy = new ProjectStateModel();
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

    public class ProjectReducer : IFlowStateReducer<ProjectStateModel>
    {
        public ProjectStateModel InitialState => new ProjectStateModel();

        public ProjectStateModel Reduce(FlowActionBase action, ProjectStateModel currentState)
        {
            ProjectStateModel newState;
            if (CurrentContext_WhenInitialMsgReceived_AddToContext(action, currentState, out newState)) return newState;
            if (CurrentContext_ChatRequestReceived_IncrementCount(action, currentState, out newState)) return newState;

            return currentState;
        }


        private bool CurrentContext_WhenInitialMsgReceived_AddToContext(FlowActionBase action, ProjectStateModel currentState, out ProjectStateModel newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ProjectActions.init(), out var initAction))
            {
                
                newState.CurrentContext.Add(new OpenAIUserMessage(initAction.Parameters));
                return true;
            }
            return false;
        }

        private bool CurrentContext_ChatRequestReceived_IncrementCount(FlowActionBase action, ProjectStateModel currentState, out ProjectStateModel newState)
        {
            newState = currentState;
            if (FlowState.IsResolvingAction(action, ProjectActions.chatRequested(), out var initAction))
            {

                newState.NumberOfChats++;
                return true;
            }
            return false;
        }
    }

    public class ProjectEffects
    {
        public ProjectEffects() { 
        }
    }
}
