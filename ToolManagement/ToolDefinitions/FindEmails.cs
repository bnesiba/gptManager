using OpenAIConnector.ChatGPTRepository.models;
using ToolManagementFlow.Models;

namespace ToolManagement.ToolDefinitions
{
    public class FindEmails: IToolDefinition
    {
        public string Name => "FindEmails";

        public string Description => "Get the latest emails that contain the search string or from address";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "SearchString",
                type = "string",
                description = "Used to search the subject and body of the emails",
                IsRequired = true
            }
            ,new ToolProperty()
            {
                name = "FromAddress",
                type = "string",
                description = "The from address of the emails to search for",
                IsRequired = false
            }

        };

        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolRequestParameters)
        {
            return new OpenAIToolMessage("THIS TOOL DOES NOT WORK", toolRequestParameters.ToolRequestId);
        }
    }
}