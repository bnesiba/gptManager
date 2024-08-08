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
        private FlowStateData<StoryEvaluatorEntity> _storyStateData;

        public StoryEvaluationController(FlowState state, FlowStateData<StoryEvaluatorEntity> stateData)
        {
            _flowState = state;
            _storyStateData = stateData;
        }


        [Route("storyTest")]
        [HttpPost]
        public virtual IActionResult StoryTest([FromBody] string message = "Once upon a time there was a dog named spot")
        {
            try
            {
                _flowState.ResolveAction(StoryEvaluatorActions.InitStoryEval(message));
                var output = _storyStateData.CurrentState(StoryEvaluatorSelectors.GetStoryEvaluation);

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