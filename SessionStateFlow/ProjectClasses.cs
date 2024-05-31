using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using SessionStateFlow.package;
using SessionStateFlow.package.Models;

namespace SessionStateFlow
{
    //TODO: This is an example. When finished flow should be in nuget package and these should be in separate files in the main project (probably).

    //TODO: find a way to unify actions

    //Actions
    //public static class ProjectActions
    //{
    //    public static FlowAction<InitialMessage> init(string initalMsg = "", Guid? chatSessionId = null) => new FlowAction<InitialMessage> { Name = "UserInitialMessage", Parameters = new InitialMessage { message = initalMsg, sessionId = chatSessionId ?? Guid.NewGuid() } };
    //    public static FlowAction<OpenAIChatRequest> chatRequested(OpenAIChatRequest? request = null) => new FlowAction<OpenAIChatRequest> { Name = "AIChatRequested", Parameters = request };
    //    public static FlowAction<OpenAIChatResponse> chatResponseReceived(OpenAIChatResponse? response = null) => new FlowAction<OpenAIChatResponse> { Name = "AIChatResponse", Parameters = response };
    //}

    //public class InitialMessage
    //{
    //    public string message { get; set; }
    //    public Guid sessionId { get; set; }
    //}

    //State Model
    //public class ProjectStateModel
    //{
    //    public List<OpenAIChatMessage> CurrentContext { get; set; }

    //    public List<string> RelatedLinks { get; set; }

    //    //TODO: remove me - testing param
    //    public  int NumberOfChats { get; set; }

    //    public ProjectStateModel()
    //    {
    //        CurrentContext = new List<OpenAIChatMessage>();
    //        RelatedLinks = new List<string>();
    //        NumberOfChats = 0;
    //    }

    //    public ProjectStateModel Copy()
    //    {
    //        var copy = new ProjectStateModel();
    //        copy.CurrentContext = CurrentContext;
    //        copy.RelatedLinks = new List<string>();
    //        copy.NumberOfChats = NumberOfChats;
    //        RelatedLinks.ForEach(l => copy.RelatedLinks.Add(l));
    //        return copy;
    //    }

    //    public override string ToString()
    //    {
    //        return $"ContextLength: {CurrentContext.Count} LinksLen: {RelatedLinks.Count} NumberOfChats: {NumberOfChats}";
    //    }
    //}

    //Selectors
    //public static class ProjectSelectors
    //{
    //    public static FlowDataSelector<ProjectStateModel, int> GetChatCount = new FlowDataSelector<ProjectStateModel, int>((stateData) => stateData.NumberOfChats);
    //}


    //TODO: update to be more like effects?
    //Reducer
    //public class ProjectReducer : IFlowStateReducer<ProjectStateModel>
    //{
    //    public ProjectStateModel InitialState => new ProjectStateModel();

    //    public ProjectStateModel Reduce(FlowActionBase action, ProjectStateModel currentState)
    //    {
    //        ProjectStateModel newState = currentState;
    //        //manage context
    //        CurrentContext_WhenInitialMsgReceived_AddToContext(action, newState, out newState);
    //        CurrentContext_WhenResponseMsgReceived_AddToContext(action, newState, out newState);

    //        //manage count
    //        CurrentContext_ChatRequestReceived_IncrementCount(action, newState, out newState);
    //        CurrentContext_ChatResponseReceived_IncrementCount(action, newState, out newState);

    //        return newState;
    //    }


    //    public void CurrentContext_WhenInitialMsgReceived_AddToContext(FlowActionBase action, ProjectStateModel currentState, out ProjectStateModel newState)
    //    {
    //        newState = currentState;
    //        if (FlowState.IsResolvingAction(action, ProjectActions.init(), out var initAction))
    //        {
    //            newState.CurrentContext.Add(new OpenAIUserMessage(initAction.Parameters.message));
    //        }
    //    }

    //    public void CurrentContext_WhenResponseMsgReceived_AddToContext(FlowActionBase action, ProjectStateModel currentState, out ProjectStateModel newState)
    //    {
    //        newState = currentState;
    //        if (FlowState.IsResolvingAction(action, ProjectActions.chatResponseReceived(), out var responseMessage))
    //        {
    //            newState.CurrentContext.Add(responseMessage.Parameters.choices[0].message);
    //        }
    //    }

    //    public void CurrentContext_ChatRequestReceived_IncrementCount(FlowActionBase action, ProjectStateModel currentState, out ProjectStateModel newState)
    //    {
    //        newState = currentState;
    //        if (FlowState.IsResolvingAction(action, ProjectActions.chatRequested(), out var _))
    //        {

    //            newState.NumberOfChats++;
    //        }
    //    }

    //    public void CurrentContext_ChatResponseReceived_IncrementCount(FlowActionBase action, ProjectStateModel currentState, out ProjectStateModel newState)
    //    {
    //        newState = currentState;
    //        if (FlowState.IsResolvingAction(action, ProjectActions.chatResponseReceived(), out var _))
    //        {

    //            newState.NumberOfChats++;
    //        }
    //    }
    //}

    //Effects
    //public class ProjectEffects: IFlowStateEffects
    //{
    //    private ChatGPTRepo _chatGPTRepo;
    //    private FlowStateData<ProjectStateModel> _flowStateData;
    //    public ProjectEffects(FlowStateData<ProjectStateModel> stateData, ChatGPTRepo chatRepo) 
    //    {
    //        _flowStateData = stateData;
    //        _chatGPTRepo = chatRepo;
    //    }

    //    List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
    //    {
    //        new FlowEffect<InitialMessage>(OnInitialMsg_CreateChatRequest_ResolveChatRequested, ProjectActions.init()),
    //        new FlowEffect<OpenAIChatRequest>(OnChatRequested_CallChatGPT_ResolveResponseReceived, ProjectActions.chatRequested())
    //    };

    //    public FlowActionBase OnInitialMsg_CreateChatRequest_ResolveChatRequested(FlowAction<InitialMessage> initialMsg)
    //    {
    //        //TODO: remove me
    //        var count = _flowStateData.CurrentState(ProjectSelectors.GetChatCount);
    //        System.Diagnostics.Debug.WriteLine("ChatCount: ", count);//TODO: removeme

    //        List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
    //        //TODO: get session context eventually to populate newcontext & potentially model
    //        newContext.Add(new OpenAIUserMessage(initialMsg.Parameters.message));
    //        OpenAIChatRequest chatRequest = new OpenAIChatRequest
    //        {
    //            model = "gpt-3.5-turbo",
    //            messages = newContext,
    //            temperature = 1
    //        };
    //        return ProjectActions.chatRequested(chatRequest);
    //    }

    //    public FlowActionBase OnChatRequested_CallChatGPT_ResolveResponseReceived(FlowAction<OpenAIChatRequest> chatRequest)
    //    {
    //        //TODO: remove me
    //        var count = _flowStateData.CurrentState(ProjectSelectors.GetChatCount);
    //        System.Diagnostics.Debug.WriteLine("ChatCount: ", count);//TODO: removeme

    //        var response = _chatGPTRepo.Chat(chatRequest.Parameters);
    //        return ProjectActions.chatResponseReceived(response);
    //    }
    //}
}
