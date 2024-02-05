using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContextManagement.Models;
using OpenAIConnector.ChatGPTRepository;
using OpenAIConnector.ChatGPTRepository.models;
using ToolManagement;

namespace ContextManagement
{
    public class ChatContextManager
    {
        //TODO: make this a database or memcache or something
        private Dictionary<Guid, ChatSession> _chatSessions = new Dictionary<Guid, ChatSession>();
        private ChatGPTRepo _chatGptRepo;
        private ToolDefinitionManager _toolManager;

        private Guid _evaluationContextId;
        private string _evaluatorIdentity;

        //private Guid _toolUseContextId;
        private string _toolUseIdentity;

        public ChatContextManager(ChatGPTRepo chatGptRepo, ToolDefinitionManager toolManager)
        {
            _chatGptRepo = chatGptRepo;
            _toolManager = toolManager;
            _evaluatorIdentity = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FixedIdentities/PromptRequirementsEvaluator.txt");
            _evaluationContextId = CreateChatSession("Evaluator", "gpt-3.5-turbo", _evaluatorIdentity).Id;

            _toolUseIdentity = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FixedIdentities/ToolUserIdentity.txt");
            //_toolUseContextId = CreateChatSession("ToolManager", "gpt-3.5-turbo", _toolUseIdentity).Id;
        }

        public ChatSession CreateStructuredChatSession(string name, string model)
        {
            string initialIdentity = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FixedIdentities/ChatPrimaryIdentity.txt");
            return CreateChatSession(name, model, initialIdentity);
        }

        public ChatSession CreateChatSession(string name, string model, string? systemMessage = null)
        {
            var chatSession = new ChatSession()
            {
                name = name,
                model = model
            };
            if (systemMessage != null)
            {
                //chatSession.AddAssistantMessage("assistant", "I am an assistant that only return json objects. Those objects only contain type and message properties");
                chatSession.AddSystemMessage("system", systemMessage);
            }
            _chatSessions.Add(chatSession.Id, chatSession);
            return chatSession;
        }

        public ChatSession? GetChatSession(Guid id)
        {
            if (_chatSessions.ContainsKey(id))
            {
                return _chatSessions[id];
            }
            else
            {
                return null;
            }
        }

        public string? Chat(Guid id, string message, int chaos = 1)
        {
            if (_chatSessions.ContainsKey(id))
            {
                //copy the context so we can add the new message without committing the user message until we get a success response
                //this might be unnecessary/easier to do another way
                List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
                _chatSessions[id].chatContext.ForEach(m => newContext.Add(m));
                newContext.Add(new OpenAIUserMessage(message));

                var chatRequest = new OpenAIChatRequest()
                {
                    model = _chatSessions[id].model,
                    messages = newContext,
                    temperature = chaos
                };
                var response = _chatGptRepo.Chat(chatRequest);

                //only update the chat context if we get a success response
                if (response != null)
                {
                    _chatSessions[id].AddUserMessage(message);
                    _chatSessions[id].AddAssistantMessage(response.choices[0].message.name, response.choices[0].message.content);
                    return response.choices[0].message.content;
                }

            }

            return null;
        }

        public string? Chat(Guid id, List<OpenAIChatMessage> additionalContext, int chaos = 1, bool ephemeral = false, int? maxTokens = null)
        {
            return GetChatResponse(id, additionalContext, chaos, ephemeral, maxTokens: maxTokens)?.choices[0].message.content;
        }



        public string? StructuredChat(Guid id, string message, int chaos = 1)
        {
            if (_chatSessions.ContainsKey(id))
            {
                //copy the context so we can add the new message without committing the user message until we get a success response
                //this might be unnecessary/easier to do another way
                List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
                _chatSessions[id].chatContext.ForEach(m => newContext.Add(m));
                newContext.Add(new OpenAIUserMessage(message));

                var chatRequest = new OpenAIChatRequest()
                {
                    model = _chatSessions[id].model,
                    messages = newContext,
                    temperature = chaos
                };
                var response = ProcessChatRequest(chatRequest);

                //only update the chat context if we get a success response
                if (response != null)
                {
                    _chatSessions[id].AddUserMessage(message);
                    _chatSessions[id].AddAssistantMessage(response.choices[0].message.name, response.choices[0].message.content);
                    return response.choices[0].message.content;
                }

            }

            return null;
        }

        private OpenAIChatResponse? GetChatResponse(Guid id, 
                                                    List<OpenAIChatMessage> additionalContext, 
                                                    int chaos = 1, 
                                                    bool ephemeral = false, 
                                                    OpenAITool[]? tools = null,
                                                    int? maxTokens = null)
        {
            if (_chatSessions.ContainsKey(id))
            {
                //copy the context so we can add the new message without committing the user message until we get a success response
                //this might be unnecessary/easier to do another way
                List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
                _chatSessions[id].chatContext.ForEach(m => newContext.Add(m));
                newContext.AddRange(additionalContext);

                var chatRequest = new OpenAIChatRequest()
                {
                    model = _chatSessions[id].model,
                    messages = newContext,
                    temperature = chaos,
                    tools = tools,
                    max_tokens = maxTokens
                };
                var response = _chatGptRepo.Chat(chatRequest);

                //only update the chat context if we get a success response and want to persist messages (non ephemeral)
                if (response != null)
                {
                    if (!ephemeral)
                    {
                        _chatSessions[id].AddMessages(additionalContext);
                        _chatSessions[id].AddAssistantMessage(response.choices[0].message.name, response.choices[0].message.content);
                    }

                    return response;
                }

            }

            return null;
        }

        private OpenAIChatResponse? ProcessChatRequest(OpenAIChatRequest chatRequest)
        {
            var tooledChatRequest = ManageToolUse(chatRequest);
            string toolInfoString = string.Empty;
            tooledChatRequest.messages.ForEach(m =>
            {
                if (m is OpenAIToolMessage)
                {
                    toolInfoString += m.content += "\n";
                }
            });
            if (!string.IsNullOrEmpty(toolInfoString))
            {
                chatRequest.messages.Add(new OpenAIAssistantMessage("ToolManager", toolInfoString));
            }

            //TODO: seems like there might be some coherence issues from keeping a non-tool instance around.
            return _chatGptRepo.Chat(chatRequest);
        }

        private OpenAIChatRequest ManageToolUse(OpenAIChatRequest chatRequest)
        {
            var toolChatRequest = chatRequest.Copy();
            var toolUserId = GenerateToolManager();
            var toolCalls = GetToolCalls(toolUserId, toolChatRequest);
            if (toolCalls.Any())
            {
                var toolResults = _toolManager.ExecuteTools(toolChatRequest.messages, toolCalls);
                toolCalls.ForEach(tc => toolChatRequest.messages.Add(new OpenAIAssistantMessage("ToolManager", "Tools", toolCalls)));
                toolResults.ForEach(tm => toolChatRequest.messages.Add(tm));
                ManageToolUse(toolChatRequest);
            }

            return toolChatRequest;
        }





        //TODO: this seems like more trouble than it's worth
        private bool AuxiliaryNeeded(OpenAIChatRequest chatRequest)
        {
            int retriesCount = 0;
            bool auxiliaryNeeded = false;
            List<OpenAIChatMessage> evaluationContext = new List<OpenAIChatMessage>()
                { new OpenAISystemMessage("Evaluator", this._evaluatorIdentity) };
            var messageEvaluationContext =chatRequest.messages.FindAll(m => m.role != OpenAIMessageRoles.system);
            var mostRecentMessage = messageEvaluationContext.Last();
            messageEvaluationContext.Remove(mostRecentMessage);
            evaluationContext.AddRange(messageEvaluationContext);

            //generate query message
            var queryMessage = new OpenAIUserMessage("'yes' or 'no', is this something you would need the internet or external systems for? " + mostRecentMessage.content);
            evaluationContext.Add(queryMessage);

            do
            {
                string? auxDecisionString = Chat(this._evaluationContextId, evaluationContext, 0, true, maxTokens:1);
                if (string.Equals(auxDecisionString, "yes",StringComparison.CurrentCultureIgnoreCase) || string.Equals(auxDecisionString, "no", StringComparison.CurrentCultureIgnoreCase))
                {
                    auxiliaryNeeded = string.Equals(auxDecisionString, "yes", StringComparison.CurrentCultureIgnoreCase);
                    break;
                }

                retriesCount++;

            } while (retriesCount<4);
            Console.WriteLine("auxiliary needed: " + auxiliaryNeeded);
            return auxiliaryNeeded;
        }

        private List<OpenAIToolCall> GetToolCalls(Guid toolUseContextId, OpenAIChatRequest chatRequest)
        {
            List<OpenAIChatMessage> toolContext = new List<OpenAIChatMessage>()
            { new OpenAISystemMessage("ToolManager", this._toolUseIdentity) };
            var messageEvaluationContext = chatRequest.messages.FindAll(m => m.role != OpenAIMessageRoles.system);
            toolContext.AddRange(messageEvaluationContext);

            var toolResponse = GetChatResponse(toolUseContextId, toolContext,tools: _toolManager.GetToolDefinitions());
            if (toolResponse != null && toolResponse.choices.Any())
            {
                return toolResponse.choices[0].message.tool_calls ?? new List<OpenAIToolCall>();
            }
            else
            {
                return new List<OpenAIToolCall>();
            }


        }

        private Guid GenerateToolManager()
        {
            var _toolUseIdentity = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FixedIdentities/ToolUserIdentity.txt");
            return CreateChatSession("ToolManager", "gpt-3.5-turbo", _toolUseIdentity).Id;
        }
    }
}
