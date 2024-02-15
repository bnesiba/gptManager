﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using Org.BouncyCastle.Asn1;

namespace ToolManagement.ToolDefinitions
{
    public class ImageEvaluate : IToolDefinition
    {
        private readonly ChatGPTRepo _chatGPTRepo;

        public ImageEvaluate(ChatGPTRepo chatGPTRepo)
        {
            _chatGPTRepo = chatGPTRepo;
        }
        public string Name => "ImageEvaluate";

        public string Description => "Interact with an AI vision model to query and understand the content of images";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "InputPrompt",
                type = "string",
                description = "Text input containing instructions/query for image(s)",
                IsRequired = true
            },
            new ArrayToolProperty()
            {
                name = "ImageArray",
                type = "array",
                items = new()
                {
                    type = "string",
                },
                description = "An Array of strings containing the urls or base64 encoded strings of an image",
                IsRequired = true
            },

        };

        //TODO: abstract more of this out? almost everything except the actual call and the response object is shared across tools
        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
        {
            Dictionary<string, string>? requestParameters = this.GetToolRequestStringParameters(toolCall);
            if (requestParameters != null)
            {
                bool toolCallArgumentsValid = this.RequestArgumentsValid(requestParameters);

                //TODO: try-catch vv
                var urlsAndBase64Strings = JsonSerializer.Deserialize<List<string>>(requestParameters["ImageArray"]);

                var inputPrompt = requestParameters["InputPrompt"];
                if (toolCallArgumentsValid)
                {
                    var  imageResponse = _chatGPTRepo.Chat(BuildVisionRequest(chatContext, inputPrompt, urlsAndBase64Strings));
                    if (imageResponse != null)
                    {
                        var outputObject = new
                        {
                            processImageSuccess = true,
                            visionResponse = imageResponse.choices[0].message.content

                        };
                        return new OpenAIToolMessage($"imageEvaluateResponse: " + JsonSerializer.Serialize(outputObject), toolCall.id);
                    }
                    else
                    {
                        return new OpenAIToolMessage("ERROR: The image evaluation failed", toolCall.id);
                    }
                   
                }
                return new OpenAIToolMessage("ERROR: Arguments to 'imageEvaluateResponse' tool were invalid or missing", toolCall.id);
            }

            return new OpenAIToolMessage("ERROR: No Arguments were provided", toolCall.id);
        }

        private OpenAIChatRequest BuildVisionRequest(List<OpenAIChatMessage> chatContext, string promptString, List<string> urlsAndBase64)
        {
            var visionContext = chatContext.FindAll(x => x.role != OpenAIMessageRoles.system).ToList();

            visionContext.Add(new OpenAIUserImageMessage(promptString, urlsAndBase64, true));

            OpenAIChatRequest visionRequest = new OpenAIChatRequest()
            {
                model = "gpt-4-vision-preview",
                messages = visionContext,
                max_tokens = 300
            };
            return visionRequest;
        }
    }
}