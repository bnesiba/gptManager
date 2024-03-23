using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;

namespace AIUtilities
{
    public class ArticleSummarizer
    {
        private readonly ChatGPTRepo _chatGptRepo;

        public ArticleSummarizer(ChatGPTRepo chatGptRepo)
        {
            _chatGptRepo = chatGptRepo;
        }

        public string? Summarize(string article, string importantContext = "", List<OpenAIChatMessage>? additionalChatContext = null)
        {
            string? summary = null;
            string importantContextString = importantContext.Length > 0 ? $"Keep this important context in mind while summarizing: {importantContext}" : string.Empty;
            List<OpenAIChatMessage> summarizerContext = new List<OpenAIChatMessage>
            {
                new OpenAISystemMessage("Summarizer", "As a document summarizer, it is your job to read and examine articles/documents and provide brief context-aware summaries of the text")
            };

            if (additionalChatContext != null)
            {
                summarizerContext.AddRange(additionalChatContext);
            }

            string summarizeInstruction =
                $"Provide an accurate and concise summary of this document/article. {importantContextString}";

            summarizerContext.Add(new OpenAIUserMessage(summarizeInstruction));


            OpenAIChatRequest summarizeRequest = new OpenAIChatRequest()
            {
                model = "gpt-3.5-turbo",
                messages = summarizerContext,
                max_tokens = 800,
                temperature = 0

            };

            OpenAIChatResponse? response = _chatGptRepo.Chat(summarizeRequest);
            if (response != null)
            {
                response.choices.ForEach(c =>
                {
                    if (summary == null)
                    {
                        summary = c.message?.content?.ToString();
                    }
                    else
                    {
                        summary += c.message?.content?.ToString();
                    }
                });
            }

            return summary;
        }
    }
}
