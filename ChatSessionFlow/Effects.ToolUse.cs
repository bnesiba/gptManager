using ChatSessionFlow.models;
using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.ChatGPTRepository;
using SessionStateFlow.package.Models;
using SessionStateFlow.package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolManagement;
using ToolManagement.ToolDefinitions.Models;

namespace ChatSessionFlow
{
    public class ToolUseEffects : IFlowStateEffects
    {
        private FlowStateData<ChatSessionEntity> _flowStateData;
        private ToolDefinitionManager _toolManager;
        private List<IToolDefinition> _definedTools;
        private FlowActionHandler _flowActionHandler;


        public ToolUseEffects(FlowActionHandler actionHandler, FlowStateData<ChatSessionEntity> flowStateData, ToolDefinitionManager toolManager, IEnumerable<IToolDefinition> definedTools)
        {
            _flowActionHandler = actionHandler;
            _flowStateData = flowStateData;
            _toolManager = toolManager;
            _definedTools = definedTools.ToList();
        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
            new FlowEffect<OpenAIChatResponse>(OnChatResponseReceived_IfToolCallsExist_ResolveToolCalls, ChatSessionActions.ChatResponseReceived()),
            new FlowEffect<List<OpenAIToolCall>>(OnToolExecutionsCompleted_ResolveChatRequested, ChatSessionActions.ToolExecutionsCompleted())
        };

        //effect methods
        public FlowActionBase OnChatResponseReceived_IfToolCallsExist_ResolveToolCalls(FlowAction<OpenAIChatResponse> chatResponseAction)
        {
            var currentContext = _flowStateData.CurrentState(ChatSessionSelectors.GetChatContext);
            List<OpenAIToolMessage> toolResults = new List<OpenAIToolMessage>();

            List<OpenAIToolCall> toolsCalled = new List<OpenAIToolCall>();
            if (chatResponseAction.Parameters != null && chatResponseAction.Parameters.HasToolCalls(out toolsCalled))
            {
                foreach (var toolCall in toolsCalled)
                {
                    var tool = _definedTools.FirstOrDefault(t => t.Name == toolCall.function.name);
                    if (tool != null)
                    {
                        Dictionary<string, string>? requestStringParameters = tool.GetToolRequestStringParameters(toolCall);
                        Dictionary<string, List<string>>? requestArrayParameters = tool.GetToolRequestArrayParameters(toolCall);
                        bool toolCallArgumentsValid = tool.RequestArgumentsValid(requestStringParameters, requestArrayParameters);

                        if (toolCallArgumentsValid)
                        {
                            var toolResult = tool.ExecuteTool(currentContext, toolCall);
                            _flowActionHandler.ResolveAction(ChatSessionActions.ToolExecutionSucceeded(toolResult));
                        }
                        else
                        {
                            _flowActionHandler.ResolveAction(ChatSessionActions.ToolExecutionFailed(new OpenAIToolMessage($"ERROR: Arguments to '{tool.Name}' tool were invalid or missing", toolCall.id)));
                        }
                    }
                }
            }
            if(toolsCalled != null && toolsCalled.Count > 0)
            {
                return ChatSessionActions.ToolExecutionsCompleted(toolsCalled);
            }
            else
            {
                return ChatSessionActions.ToolsExecutionEmpty();
            }
        }

        public FlowActionBase OnToolExecutionsCompleted_ResolveChatRequested(FlowAction<List<OpenAIToolCall>> chatResponseAction)
        {
            var currentContext = _flowStateData.CurrentState(ChatSessionSelectors.GetChatContext);
            //TODO: get session context eventually to populate newcontext & potentially model
            OpenAIChatRequest chatRequest = new OpenAIChatRequest
            {
                model = "gpt-3.5-turbo",
                //model = "gpt-4o",
                messages = currentContext,
                temperature = 1,
                tools = _toolManager.GetToolDefinitions()
            };
            return ChatSessionActions.ChatRequested(chatRequest);
        }


    }
}
