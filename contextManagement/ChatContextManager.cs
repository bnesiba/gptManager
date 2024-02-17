
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

        private string _toolUseIdentity;

        public ChatContextManager(ChatGPTRepo chatGptRepo, ToolDefinitionManager toolManager)
        {
            _chatGptRepo = chatGptRepo;
            _toolManager = toolManager;
            _evaluatorIdentity = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FixedIdentities/PromptRequirementsEvaluator.txt");
            _evaluationContextId = CreateChatSession("Evaluator", "gpt-3.5-turbo", _evaluatorIdentity).Id;

            _toolUseIdentity = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FixedIdentities/ToolUserIdentity.txt");
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
                    _chatSessions[id].AddAssistantMessage(response.choices[0].message.content.ToString());
                    return response.choices[0].message.content.ToString();
                }

            }

            return null;
        }

        public string? Chat(Guid id, List<OpenAIChatMessage> additionalContext, int chaos = 1, bool ephemeral = false, int? maxTokens = null)
        {
            return GetChatResponse(id, additionalContext, chaos, ephemeral, maxTokens: maxTokens)?.choices[0].message.content.ToString();
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
                    _chatSessions[id].AddAssistantMessage(response?.content.ToString() ?? "");
                    return response?.content.ToString();
                }

            }

            return null;
        }

        public string? StructuredImageChat(Guid id, string userMessage, List<string> imageUrls, int chaos = 1)
        {
            if (_chatSessions.ContainsKey(id))
            {
                //copy the context so we can add the new message without committing the user message until we get a success response
                //this might be unnecessary/easier to do another way
                List<OpenAIChatMessage> newContext = new List<OpenAIChatMessage>();
                _chatSessions[id].chatContext.ForEach(m => newContext.Add(m));
                string imagePrompt = "The user has provided images as a part of their prompt. *Use the the ImageEvaluate tool to process the ImageArray and InputPrompt*.";
                imagePrompt += "The user has provided the following ImageArray: [ ";
                imagePrompt += string.Join(", ", imageUrls);
                imagePrompt += " ]";

                newContext.Add(new OpenAIUserMessage(imagePrompt));
                newContext.Add(new OpenAIUserMessage(userMessage));

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
                    _chatSessions[id].AddUserMessage(userMessage);
                    _chatSessions[id].AddAssistantMessage(response?.content.ToString() ?? "");
                    return response?.content.ToString();
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
                        _chatSessions[id].AddMessage(response.choices[0].message);
                    }

                    return response;
                }

            }

            return null;
        }

        private OpenAIAssistantMessage? ProcessChatRequest(OpenAIChatRequest chatRequest)
        {
            var tooledChatRequest = ManageToolUse(chatRequest);
            return tooledChatRequest.messages.FindLast(f => f.role == OpenAIMessageRoles.assistant) as OpenAIAssistantMessage;
        }

        private OpenAIChatRequest ManageToolUse(OpenAIChatRequest chatRequest)
        {
            var toolChatRequest = chatRequest.Copy();
            toolChatRequest.messages.RemoveAll(m => m.role == OpenAIMessageRoles.system);
            var toolUserId = GenerateToolManager();
            OpenAIAssistantMessage? chatResponse = null;

            var toolCalls = GetToolCalls(toolUserId, toolChatRequest,out chatResponse);
            if (toolCalls.Any())
            {
                var toolResults = _toolManager.ExecuteTools(toolChatRequest.messages, toolCalls);

                if (chatResponse != null)
                {
                    toolChatRequest.messages.Add(chatResponse);
                }
                toolResults.ForEach(tm => toolChatRequest.messages.Add(tm));
                return ManageToolUse(toolChatRequest);
            }
            else if(chatResponse != null)
            {
                toolChatRequest.messages.Add(chatResponse);
            }

            return toolChatRequest;
        }

        private List<OpenAIToolCall> GetToolCalls(Guid toolUseContextId, OpenAIChatRequest chatRequest, out OpenAIAssistantMessage? responseMessage)
        {
            List<OpenAIChatMessage> toolContext = new List<OpenAIChatMessage>();
            //{ new OpenAISystemMessage("ToolManager", this._toolUseIdentity) };
            var messageEvaluationContext = chatRequest.messages.FindAll(m => m.role != OpenAIMessageRoles.system);
            toolContext.AddRange(messageEvaluationContext);

            var toolResponse = GetChatResponse(toolUseContextId, toolContext,tools: _toolManager.GetToolDefinitions());
            responseMessage = null;
            if (toolResponse != null && toolResponse.choices.Any())
            {
                responseMessage = toolResponse.choices[0].message;
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
