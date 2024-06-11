using ActionFlow;
using ChatSessionFlow;
using ChatSessionFlow.models;
using Microsoft.AspNetCore.Mvc;
using OpenAIConnector.ChatGPTRepository;

namespace gptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatGPTRepo _chatGPTRepo;
        private FlowState _flowState;
        private FlowStateData<ChatSessionEntity> _chatStateData;

        public ChatController(ChatGPTRepo chatGPTRepo, FlowState state, FlowStateData<ChatSessionEntity> stateData)
        {
            _chatGPTRepo = chatGPTRepo;
            _flowState = state;
            _chatStateData = stateData;
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

        [Route("reduxTest")]
        [HttpPost]
        public virtual IActionResult TEST([FromBody] string message = "hello")
        {
            try
            {
                _flowState.ResolveAction(ChatSessionActions.Init(message));
                //_flowState.ResolveAction(ChatSessionActions.ResponseValidatonRequested());
                var lastMsg = _chatStateData.CurrentState(ChatSessionSelectors.GetLatestMessage);

                return Ok(lastMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }

        [Route("imageTest")]
        [HttpPost]
        public virtual IActionResult IMGTEST()
        {
            try
            {
                
                var imageURL =
                    "https://i.ibb.co/7Gccsmb/gyges-of-lydia-raccoon-wizard-programming-a-pumpkin-computer-f99ab486-7eed-4635-90a5-e64b9016601d.png";

                //var image2Url = "https://i.ibb.co/0GGRDFC/0-0.png";//apparently 0-0 is an invalid name and will cause bad requests.
                var image2Url = "https://i.ibb.co/F6ckW20/gyges-of-lydia-Raccoon-wearing-brimmed-pointy-wizard-hat-progr-8090027b-a586-4681-829f-7070f506fa96.png";

                string msg =
                    "please describe the setting, content, and emotional feel of each image, then provide a comparison of the two images with an emphasis on differences in setting, content, and tone. Email the result to bnesiba@gmail.com";


                //var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-3.5-turbo");
                //var result =  _chatContextManager.StructuredImageChat(chatSession.Id, msg, new List<string>(){ imageURL,image2Url });


                return Ok();
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
                //var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-3.5-turbo");
                //var chatSession = _chatContextManager.CreateStructuredChatSession($"Chat-{DateTime.Now}", "gpt-4-turbo-preview");

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }
    }
}
