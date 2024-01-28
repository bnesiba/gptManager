using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using gptManager.Repository.ChatGPTRepository;
using gptManager.Repository.ChatGPTRepository.models;

namespace gptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatGPTRepo _chatGPTRepo;

        public ChatController(ChatGPTRepo chatGPTRepo)
        {
            _chatGPTRepo = chatGPTRepo;
        }

        [HttpGet]
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
                var chatResult = _chatGPTRepo.SimpleChat(chatMessage);
                if (chatResult != null)
                {
                    return Ok(chatResult);
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
    }
}
