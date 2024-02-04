﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions
{
    public class FindEmails: ToolDefinition
    {
        public string Name => "FindEmails";

        public string Description => "Get the latest emails that contain the search string or from address";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "SearchString",
                type = "string",
                description = "Used to search the subject and body of the emails"
            }
            ,new ToolProperty()
            {
                name = "FromAddress",
                type = "string",
                description = "The from address of the emails to search for"
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
        {
            return new OpenAIToolMessage("stuff", toolCall.id);
        }
    }
}