using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository.models;

namespace ToolManagement.ToolDefinitions
{
    public class SendEmail: IToolDefinition
    {
        private readonly GmailConnector _emailConnector;

        public SendEmail(GmailConnector emailConnector)
        {
            _emailConnector = emailConnector;
        }
        public string Name => "SendEmail";

        public string Description => "Send an email from the preconfigured address by defining the ToAddress, Subject and Body of the email";

        public List<ToolProperty> InputParameters => new List<ToolProperty>()
        {
            new ToolProperty()
            {
                name = "ToAddress",
                type = "string",
                description = "The email address to send the email to",
                IsRequired = true
            },
            new ToolProperty()
            {
                name = "Subject",
                type = "string",
                description = "The subject of the email",
                IsRequired = true
            },
            new ToolProperty()
            {
                name = "Content",
                type = "string",
                description = "The body of the email",
                IsRequired = true
            }

        };

        //TODO: abstract more of this out? everything except the actual call and the response object is shared across tools
        public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
        {
            Dictionary<string, string>? requestParameters = this.GetToolRequestParameters(toolCall);
            if (requestParameters != null)
            {
                bool toolCallArgumentsValid = this.RequestArgumentsValid(requestParameters);

                if (toolCallArgumentsValid)
                {
                    _emailConnector.SendEmail(requestParameters["ToAddress"], requestParameters["Subject"], requestParameters["Content"]);
                    var outputObject = new
                    {
                        sendEmailSuccess = true,
                        toAddress = requestParameters["ToAddress"],
                        subject = requestParameters["Subject"]

                    };
                    return new OpenAIToolMessage($"sendEmailResponse: " + JsonSerializer.Serialize(outputObject), toolCall.id);
                }
                return new OpenAIToolMessage("ERROR: Arguments to 'SendEmail' tool were invalid or missing", toolCall.id);
            }

            return new OpenAIToolMessage("ERROR: No Arguments were provided", toolCall.id);
        }
    }
}