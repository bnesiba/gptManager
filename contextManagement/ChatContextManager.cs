using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContextManagement.Models;
using gptManager.Repository.ChatGPTRepository;
using gptManager.Repository.ChatGPTRepository.models;

namespace ContextManagement
{
    public class ChatContextManager
    {
        //TODO: make this a database or memcache or something
        private Dictionary<Guid, ChatSession> _chatSessions = new Dictionary<Guid, ChatSession>();
        private ChatGPTRepo _chatGPTRepo;

        public ChatContextManager(ChatGPTRepo chatGPTRepo)
        {
            _chatGPTRepo = chatGPTRepo;
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

        public string? Chat(Guid id, string message)
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
                    messages = newContext
                };
                var response = _chatGPTRepo.Chat(chatRequest);

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
    }
}
