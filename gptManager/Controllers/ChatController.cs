using ContextManagement;
using gptManager.Repository.ChatGPTRepository;
using gptManager.Repository.ChatGPTRepository.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatGPTRepo _chatGPTRepo;
        private readonly ChatContextManager _chatContextManager;

        public ChatController(ChatGPTRepo chatGPTRepo, ChatContextManager chatContextManager)
        {
            _chatGPTRepo = chatGPTRepo;
            _chatContextManager = chatContextManager;
        }

        [HttpGet]
        [Route("models")]
        public virtual IActionResult GetAvailableModels()
        {
            try
            {
                var modelsResult = _chatGPTRepo.GetModels();
                if (modelsResult != null)
                {
                    return Ok(modelsResult);
                }
                else
                {
                    return StatusCode(400, modelsResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public virtual IActionResult SimpleChat([FromBody] string chatMessage)
        {
            try
            {
                //var chatResult = _chatGPTRepo.SimpleChat(chatMessage);
                var chatSession = _chatContextManager.CreateChatSession($"Chat-{DateTime.Now}", "gpt-3.5-turbo");
                var chatResult = _chatContextManager.Chat(chatSession.Id, chatMessage);
                
                if (chatResult != null)
                {
                    Tuple<Guid, string> chatResponse = new Tuple<Guid, string>(chatSession.Id, chatResult);
                    return Ok(chatResponse);
                }
                else
                {
                    return StatusCode(400, chatResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("conversate/{conversationId}")]
        public virtual IActionResult AdvancedChat([FromBody] string chatMessage, [FromRoute] Guid conversationId)
        {
            try
            {
                //TODO: implement conversationID
                //var chatResult = _chatGPTRepo.AdvancedChat("",chatMessage,new List<OpenAIChatMessage>());
                var chatSession = _chatContextManager.GetChatSession(conversationId);
                if (chatSession != null)
                {
                    var chatResult = _chatContextManager.Chat(chatSession.Id, chatMessage);

                    if (chatResult != null)
                    {
                        return Ok(chatResult);
                    }
                    else
                    {
                        return StatusCode(400, chatResult);
                    }
                }
                else
                {
                    return StatusCode(404, $"No chat session found for id: {conversationId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }
    }
}
