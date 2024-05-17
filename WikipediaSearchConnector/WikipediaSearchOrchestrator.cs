using AIUtilities;
using Newtonsoft.Json;
using OpenAIConnector.ChatGPTRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;
using WikipediaSearchConnector.Models;
using ToolManagement.ToolDefinitions;
using GoogleCloudConnector.GmailAccess;

namespace WikipediaSearchConnector
{
    public class WikipediaSearchOrchestrator
    {
        private GoogleCustomWikiSearchConnector _searchConnector;
        private WikipediaConnector _wikiConnector;
        private ArticleSummarizer _articleSummarizer;
        private ChatGPTRepo _chatGPTRepo;

        public WikipediaSearchOrchestrator(GoogleCustomWikiSearchConnector searchConnector,WikipediaConnector wikiConnector, ArticleSummarizer articleSummarizer, ChatGPTRepo chatGPTRepo)
        {
            _searchConnector = searchConnector;
            _wikiConnector = wikiConnector;
            _articleSummarizer = articleSummarizer;
            _chatGPTRepo = chatGPTRepo;
        }

        /// <summary>
        /// Search wikipedia.org using a custom google search and then using wikipedia.org API to get the content of the wiki page and summarize it
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <param name="additionalContext"></param>
        /// <returns></returns>
        public string? Search(string query, int count = 3, string additionalContext = null)
        {
            //find relevant wiki pages
            var searchResults = _searchConnector.Search(query, count)?.items ?? new List<GoogleCustomSearchResult>();
            List<string> relevantSummaries = new List<string>();

            foreach (var wikiPage in searchResults)
            {
                //get the content of the wiki page
                var wikiExtract = _wikiConnector.GetExtracts(wikiPage.GetArticleTitle());
                if (wikiExtract != null)
                {
                    //summarize the content keeping the search query/purpose in mind
                    var summary = _articleSummarizer.Summarize(wikiExtract.query.pages.First().Value.extract, query);
                    var isRelevant = StringIsRelevant(summary, query, additionalContext);
                    if (isRelevant == true)
                    {
                        relevantSummaries.Add(summary);
                    }
                    //determine if result is helpful? - could do this at the end, but picking multiples might be harder
                    //if it is add it to list.
                }
                //return relevant summarized results
            }

            return null;
        }

        private bool? StringIsRelevant(string toEvaluate, string? prompt, string? additionalContext)
        {

            List<OpenAIChatMessage> context = new List<OpenAIChatMessage>(){new OpenAISystemMessage("You will be given two sets of information. One will be an article or something and the other will be a query or prompt. You determine if the document is necessary to complete the prompt and use tools to mark it accordingly.")};
            if (additionalContext != null)
            {
                context.Add(new OpenAIUserMessage(additionalContext));
            }
            context.Add(new OpenAIUserMessage($"Is the following content relevant to completing this prompt: '{prompt}'? \nContent: \n{toEvaluate}"));

            OpenAIChatRequest request = new OpenAIChatRequest()
            {
                model = "gpt-3.5-turbo",
                messages = context, 
                temperature = 0,
                tools = new List<OpenAITool>(){new RelevanceDetector().GetToolRequestDefinition()}.ToArray(),
                tool_choice = new RelevanceDetector().GetToolRequestDefinition()
            };

            var result = _chatGPTRepo.Chat(request);
            if (result != null && result.choices.Count > 0)
            {
                var response = result.choices[0]?.message?.content?.ToString() ?? "";
                if (response.Contains("true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return null;
        }

        private class RelevanceDetector : IToolDefinition
        {
            public string Name => "MarkRelevance";

            public string Description =>
                "Mark the provided content relevant if necessary/useful to resolve the prompt; otherwise mark it irrelevant";

            public List<ToolProperty> InputParameters => new List<ToolProperty>
            {
                new EnumToolProperty()
                {
                    name = "IsRelevant",
                    type = "string",
                    enumValues = new List<string>{"true", "false"},
                    description = "'true' if the string is relevant/useful to resolving the prompt; 'false' otherwise",
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
                            isRelevant= requestParameters["IsRelevant"],


                        };
                        return new OpenAIToolMessage($"sendEmailResponse: " + JsonSerializer.Serialize(outputObject), toolCall.id);
                    }
                    return new OpenAIToolMessage("ERROR: Arguments to 'SendEmail' tool were invalid or missing", toolCall.id);
                }

                return new OpenAIToolMessage("ERROR: No Arguments were provided", toolCall.id);
            }
        }
    }
}
