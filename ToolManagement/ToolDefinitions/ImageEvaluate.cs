using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using Org.BouncyCastle.Asn1;
using ToolManagementFlow.Models;

namespace ToolManagement.ToolDefinitions
{
    public class ImageEvaluate : IToolDefinition
    {
        private readonly ChatGPTRepo _chatGPTRepo;

        //static accessor for Tool Management
        public static string ToolName => "ImageEvaluate";

        public ImageEvaluate(ChatGPTRepo chatGPTRepo)
        {
            _chatGPTRepo = chatGPTRepo;
        }
        public string Name => ToolName;

        public string Description => "Query an advanced image AI for info about images. YOU MUST PROVIDE: 'InputPrompt' and 'ImageArray' parameters. USE THIS ON IMAGES and image urls";

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
                description = "An Array of strings containing the urls or base64 encoded strings of images",
                IsRequired = true
            },

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            var imageResponse = _chatGPTRepo.Chat(BuildVisionRequest(chatContext, toolParams.GetStringParam("InputPrompt"), toolParams.GetStringArrayParam("ImageArray")));
            if (imageResponse != null)
            {
                var outputObject = new
                {
                    processImageSuccess = true,
                    visionResponse = imageResponse.choices[0].message.content

                };
                return new OpenAIToolMessage($"imageEvaluateResponse: " + JsonSerializer.Serialize(outputObject), toolParams.ToolRequestId);
            }
            else
            {
                return new OpenAIToolMessage("ERROR: The image evaluation failed", toolParams.ToolRequestId);
            }
        }

        //TODO: find a way to avoid having to use this type?
        private OpenAIImageChatRequest BuildVisionRequest(List<OpenAIChatMessage> chatContext, string promptString, List<string> urlsAndBase64)
        {
            var visionContext = chatContext.FindAll(x => x.role != OpenAIMessageRoles.system).ToList();
            //var visionContext = new List<OpenAIChatMessage>();

            visionContext.Add(new OpenAIUserImageMessage(promptString, urlsAndBase64, true));

            OpenAIImageChatRequest visionRequest = new OpenAIImageChatRequest()
            {
                //model = "gpt-4-vision-preview",
                model = "gpt-4o",
                messages = visionContext,
                max_tokens = 1500
            };
            return visionRequest;
        }
    }
}