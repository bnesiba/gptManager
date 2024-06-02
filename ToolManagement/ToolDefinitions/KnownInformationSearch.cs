﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;


namespace ToolManagement.ToolDefinitions
{
    public class KnownInformationSearch : IToolDefinition
    {
        private ChatGPTRepo _chatGPTRepo;

        public string Name => "KnownInformationSearch";

        public string Description => "Ask ChatGPT for information - This can only return information that ChatGPT is aware of.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
            {
                new ToolProperty()
                {
                    name = "query",
                    type = "string",
                    description = "Text describing desired information",
                    IsRequired = true
                },
                new EnumToolProperty()
                {
                    name = "responseLength",
                    type = "string",
                    description = "general length the response is allowed to be. Only use 'long' if you expect to need a lot of information.",
                    enumValues = new List<string>{"small", "medium", "large"},
                    IsRequired = true
                }
            };

        public KnownInformationSearch(ChatGPTRepo chatGPTRepo)
        {
            _chatGPTRepo = chatGPTRepo;
        }

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
        {
            Dictionary<string, string>? requestParameters = this.GetToolRequestStringParameters(toolCall);
            if (requestParameters != null)
            {
                bool toolCallArgumentsValid = this.RequestArgumentsValid(requestParameters);

                if (toolCallArgumentsValid)
                {
                    //process query here

                    var outputObject = new
                    {
                        results = "this isn't implemented this way - sry."
                    };
                    return new OpenAIToolMessage($"KnownInformationSearch:" + JsonConvert.SerializeObject(outputObject), toolCall.id);
                }
                return new OpenAIToolMessage("ERROR: Arguments to 'KnownInformationSearch' tool were invalid or missing", toolCall.id);
            }

            return new OpenAIToolMessage("ERROR: No Arguments were provided", toolCall.id);
        }

        //new and improved (simplified) tool call 
        //TODO: Eventually remove the other one
        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {
            //process query here
            List<OpenAIChatMessage> newChatContext = new List<OpenAIChatMessage>();
            newChatContext.Add(new OpenAIUserMessage(toolParams.GetStringParam("query")));

            OpenAIChatRequest openAIChatRequest = new OpenAIChatRequest
            {
                model = "gpt-3.5-turbo",//TODO: make these a const or something - magic strings bad.
                                        //model = "gpt-4o",
                messages = newChatContext,
                temperature = 1,
                max_tokens = GetMaxTokens(toolParams.GetStringParam("responseLength"))
            };
            object outputObject;
            var knowledgeResponse = _chatGPTRepo.Chat(openAIChatRequest);
            if (knowledgeResponse != null)
            {
                outputObject = new
                {
                    KnownInformationQuery = toolParams.GetStringParam("query"),
                    KnownInformationResult = knowledgeResponse.choices[0].message.content
                };
            }
            else
            {
                outputObject = new
                {
                    KnownInformationQuery = toolParams.GetStringParam("query"),
                    KnownInformationResult = "An error occurred. Unable to query known information - try another tool or query"
                };
            }
            return new OpenAIToolMessage($"KnownInformationSearchResponse: " + JsonConvert.SerializeObject(outputObject), toolParams.ToolRequestId);

        }

        private int GetMaxTokens(string responseLength)
        {
            int maxTokens = 250;

            switch (responseLength)
            {
                case "small":
                    return 250;
                case "medium":
                    return 800;
                case "large":
                    return 1200;
                default:
                    return 250;
            }
        }
    }
}

