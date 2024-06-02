using System.Text.Json;
using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement.ToolDefinitions.Models;

namespace AIUtilities
{
    public class ArticleSummarizer
    {
        private readonly ChatGPTRepo _chatGptRepo;

        public ArticleSummarizer(ChatGPTRepo chatGptRepo)
        {
            _chatGptRepo = chatGptRepo;
        }

        public string? Summarize(string article, string importantContext = "", int tokenLimit = 200, List<OpenAIChatMessage>? additionalChatContext = null)
        {
            string? summary = null;
            string importantContextString = importantContext.Length > 0 ? $"Keep this important context in mind while summarizing: {importantContext}." : string.Empty;
            List<OpenAIChatMessage> summarizerContext = new List<OpenAIChatMessage>
            {
                new OpenAISystemMessage("Summarizer", "As a document summarizer, it is your job to read and examine articles/documents and provide brief context-aware summaries of the text")
            };

            if (additionalChatContext != null)
            {
                summarizerContext.AddRange(additionalChatContext);
            }

            string summarizeInstruction =
                $"Provide an accurate and concise summary of the document/article. {importantContextString}. \n\n {article}";

            summarizerContext.Add(new OpenAIUserMessage(summarizeInstruction));


            OpenAIChatRequest summarizeRequest = new OpenAIChatRequest()
            {
                model = "gpt-3.5-turbo",
                messages = summarizerContext,
                max_tokens = tokenLimit,
                temperature = 0

            };

            OpenAIChatResponse? response = _chatGptRepo.Chat(summarizeRequest);
            if (response is { choices: { Count: > 0 } }) //this null checks response and choices and verifies there are choices
            {
                summary = response.choices[0].message?.content?.ToString() ?? string.Empty;
            }

            return summary;
        }

        private class SummaryFinalizer : IToolDefinition
        {
            public string Name => "FinalizeSummary";

            public string Description =>
                "Provide the final summary of the document";

            public List<ToolProperty> InputParameters => new List<ToolProperty>
            {
                new ToolProperty()
                {
                    name = "DocumentSummary",
                    type = "string",
                    description = "Your final summary of the provided document, taking any additional information into account",
                    IsRequired = true
                },
                new ToolProperty()
                {
                    name = "SummaryDataRelationship",
                    type = "string",
                    description = "Your explanation of how the summary is related to the additional data if relevant",
                    IsRequired = true
                }
            };

            public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, OpenAIToolCall toolCall)
            {
                Dictionary<string, string>? requestParameters = this.GetToolRequestStringParameters(toolCall);
                if (requestParameters != null)
                {
                    bool toolCallArgumentsValid = this.RequestArgumentsValid(requestParameters);

                    if (toolCallArgumentsValid)
                    {

                        var outputObject = new
                        {
                            sunmmary = requestParameters["DocumentSummary"],


                        };
                        return new OpenAIToolMessage($"sendEmailResponse: " + JsonSerializer.Serialize(outputObject), toolCall.id);
                    }
                    return new OpenAIToolMessage("ERROR: Arguments to 'FinalizeSummary' tool were invalid or missing", toolCall.id);
                }

                return new OpenAIToolMessage("ERROR: No Arguments were provided", toolCall.id);
            }

            public OpenAIToolMessage ExecuteTool(List<OpenAIChatMessage> chatContext, ToolRequestParameters toolRequestParameters)
            {
                throw new NotImplementedException();
            }
        }
    }
}
