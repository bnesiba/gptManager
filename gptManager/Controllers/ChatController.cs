using ContextManagement;
using GoogleCloudConnector.GmailAccess;
using Microsoft.AspNetCore.Mvc;
using OpenAIConnector.ChatGPTRepository;

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

        [Route("test")]
        [HttpPost]
        public virtual IActionResult TEST([FromBody] string emailBody = "hello")
        {
            try
            {
                string msg =
                    "how is the news? find and summarize some of the latest news, then email me (bnesiba@gmail.com) the summary and an analysis of how worries I ought to be";
                var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-3.5-turbo");
                return this.StructuredChat(msg, chatSession.Id);


                return Ok();
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
                var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-3.5-turbo");
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

        [HttpGet]
        [Route("newSession")]
        public virtual IActionResult GetNewSessionId()
        {
            try
            {
                var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-3.5-turbo");
                //var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-4-turbo-preview");

                return Ok(chatSession.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("structuredChat/{conversationId}")]
        public virtual IActionResult StructuredChat([FromBody] string chatMessage, [FromRoute] Guid conversationId)
        {
            try
            {
                //TODO: consider putting an event-like layer between the api controller stuff and the context manager
                //so that it can be used when I get around to making this an android app.
                var chatSession = _chatContextManager.GetChatSession(conversationId);
                if (chatSession != null)
                {
                    var chatResult = _chatContextManager.StructuredChat(chatSession.Id, chatMessage);

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
