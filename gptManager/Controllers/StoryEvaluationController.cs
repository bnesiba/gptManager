using ActionFlow;
using ChatSessionFlow;
using ChatSessionFlow.models;
using Microsoft.AspNetCore.Mvc;
using OpenAIConnector.ChatGPTRepository;
using StoryEvaluatorFlow;
using StoryEvaluatorFlow.Models;

namespace gptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryEvaluationController : ControllerBase
    {
        private FlowState _flowState;
        private FlowStateData<StoryEvaluatorEntity> _storyStoryStateData;
        private FlowStateData<ChatSessionEntity> _chatStateData;

        public StoryEvaluationController(FlowState state, FlowStateData<StoryEvaluatorEntity> storyStateData, FlowStateData<ChatSessionEntity> chatStateData)
        {
            _flowState = state;
            _storyStoryStateData = storyStateData;
            _chatStateData = chatStateData;
        }


        [Route("storyEvalTest")]
        [HttpPost]
        public virtual IActionResult StoryTest([FromBody] string message = "Once upon a time there was a dog named spot")
        {
            try
            {
                _flowState.ResolveAction(StoryEvaluatorActions.InitStoryEval(message));
                var output = _storyStoryStateData.CurrentState(StoryEvaluatorSelectors.GetStoryEvaluation);

                return Ok(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }

        [Route("storyRAGTest")]
        [HttpPost]
        public virtual IActionResult StorySearchTest([FromBody] string message = "Find some stories with dogs...")
        {
            try
            {
                _flowState.ResolveAction(StoryEvaluatorActions.InitStoryChat(message));
                var output = _chatStateData.CurrentState(ChatSessionSelectors.GetLatestMessage);

                return Ok(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred\n\n {ex}");
                return StatusCode(500);
            }
        }



    }
}