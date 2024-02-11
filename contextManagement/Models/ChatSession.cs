using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAIConnector.ChatGPTRepository.models;

namespace ContextManagement.Models
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public List<OpenAIChatMessage> chatContext { get; set; }
        public string model { get; set; }
        public string name { get; set; }

        public ChatSession()
        {
            Id = Guid.NewGuid();
            chatContext = new List<OpenAIChatMessage>();
        }

        public void AddUserMessage(string message)
        {
            chatContext.Add(new OpenAIUserMessage(message));
        }

        public void AddSystemMessage(string? systemName, string message)
        {
            chatContext.Add(new OpenAISystemMessage(systemName, message));
        }

        public void AddAssistantMessage(string message)
        {
            chatContext.Add(new OpenAIAssistantMessage(message));
        }

        public void AddMessage(OpenAIChatMessage message)
        {
            chatContext.Add(message);
        }

        public void AddMessages(List<OpenAIChatMessage> messages)
        {
            chatContext.AddRange(messages);
        }
    }
}
