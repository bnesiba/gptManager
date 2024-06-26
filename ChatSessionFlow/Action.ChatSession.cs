﻿using ActionFlow.Models;
using ChatSessionFlow.models;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace ChatSessionFlow
{
    public static class ChatSessionActions
    {
        public static FlowAction<InitialMessage> Init(string initalMsg = "", Guid? chatSessionId = null) => new FlowAction<InitialMessage> { Name = "UserInitialMessage", Parameters = new InitialMessage { message = initalMsg, sessionId = chatSessionId ?? Guid.NewGuid() } };
        public static FlowAction<OpenAIChatRequest> ChatRequested(OpenAIChatRequest? request = null) => new FlowAction<OpenAIChatRequest> { Name = "AIChatRequested", Parameters = request };
        public static FlowAction<OpenAIChatResponse> ChatResponseReceived(OpenAIChatResponse? response = null) => new FlowAction<OpenAIChatResponse> { Name = "AIChatResponse", Parameters = response };
        public static FlowAction<ToolRequestParameters> ToolExecutionRequested(ToolRequestParameters? toolRequest = null) => new FlowAction<ToolRequestParameters> { Name = "ToolExecutionRequested", Parameters = toolRequest };
        public static FlowAction<OpenAIToolMessage> ToolExecutionSucceeded(OpenAIToolMessage? toolMessage = null) => new FlowAction<OpenAIToolMessage> { Name = "ToolExecutionSucceeded", Parameters = toolMessage };
        public static FlowAction<OpenAIToolMessage> ToolExecutionFailed(OpenAIToolMessage? toolMessage = null) => new FlowAction<OpenAIToolMessage> { Name = "ToolExecutionFailed", Parameters = toolMessage };
        public static FlowAction<List<OpenAIToolCall>> ToolExecutionsCompleted(List<OpenAIToolCall>? toolsRequested = null) => new FlowAction<List<OpenAIToolCall>> { Name = "ToolExecutionCompleted", Parameters = toolsRequested };
        public static FlowAction ToolsExecutionEmpty() => new FlowAction { Name = "NoToolsExecuted"};
        //public static FlowAction ResponseValidatonRequested() => new FlowAction { Name = "ResponseValidationRequested" };

    }
}
