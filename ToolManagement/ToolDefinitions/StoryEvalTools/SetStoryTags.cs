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
using ToolManagement.ToolDefinitions.Models;

namespace ToolManagement.ToolDefinitions.StoryEvalTools
{
    public class SetStoryTags : IToolDefinition
    {

        //static accessor for Tool Management
        public static string ToolName => "SetStoryTags";

        public string Name => ToolName;

        public string Description => "Add various types of tags to the story. These tags identify themes and objects of interest in the story and are used to relate and filter different stories.";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ArrayToolProperty()
            {
                name = "Animals",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "What kinds of animals were present in the story",
                    enumValues = new List<string>{"dog", "cat", "bird", "snake", "bug"},
                    IsRequired = true
                },
                description = "An array of strings representing types of animals that were present in the story",
                IsRequired = true
            },
            new ArrayToolProperty()
            {
                name = "Vehicles",
                type = "array",
                items =  new EnumToolProperty()
                {
                    type = "string",
                    description = "What kinds of vehicles were present in the story",
                    enumValues = new List<string>{"car", "truck", "boat", "plane", "bike"},
                    IsRequired = true
                },
                description = "An array of strings representing types of vehicles that were present in the story",
                IsRequired = true
            },
            new EnumToolProperty()
            {
                name = "ReadingLevel",
                type = "string",
                description = "What age-group is this story most appropriate for?",
                enumValues = new List<string>{"newborn", "toddler", "child", "young adult", "adult", "old person"},
                IsRequired = true
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolParams)
        {

            return new OpenAIToolMessage($"SetStoryTags: " + "Thanks!", toolParams.ToolRequestId);
        }
    }
}