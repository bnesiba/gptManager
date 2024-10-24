using ActionFlow;
using ImageGenFlow;
using ImageGenFlow.Models;
using Microsoft.AspNetCore.Mvc;

namespace gptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private FlowState _flowState;
        private FlowStateData<ImageGenStateEntity> _imgGenStateData;

        public ImageController(FlowState state, FlowStateData<ImageGenStateEntity> imgGenStateData)
        {
            _flowState = state;
            _imgGenStateData = imgGenStateData;
        }


        [Route("GenerateImage")]
        [HttpPost]
        public virtual IActionResult ImgGenTest([FromBody] string prompt = "An enchanting forest scene, on a bright summer day." +
            " A raccoon, wearing a large brimmed pointy stereotypical wizard's hat with stars and moons. Designing a friendly worker-robot with his magic and engineering skill.")
        {
            try
            {
                _flowState.ResolveAction(ImageGenActions.InitImageGen(prompt));
                var output = _imgGenStateData.CurrentState(ImageGenSelectors.GetGeneratedImages);

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