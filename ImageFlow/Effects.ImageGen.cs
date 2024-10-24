using OpenAIConnector.ChatGPTRepository.models;
using ActionFlow.Models;
using ActionFlow;
using ImageGenFlow.Models;
using OpenAIConnector.DallERepository.models;
using OpenAIConnector.DallERepository;

namespace ImageGenFlow
{
    public class ImageGenEffects : IFlowStateEffects
    {
        private FlowActionHandler _flowActionHandler;
        private DallERepo _dallERepo;



        public ImageGenEffects(FlowActionHandler flowHandler, DallERepo dallERepo)
        {
            _flowActionHandler = flowHandler;
            _dallERepo = dallERepo;
        }

        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnInitImageGen_CreateImageGenerationRequest_ResolveImageGenerationRequested, ImageGenActions.InitImageGen()),
           this.effect(OnImageGenerationRequested_RequestImageGen_ResolveImageGeneration, ImageGenActions.ImageGenRequested()),

        };

        //Effect Methods
        public FlowActionBase OnInitImageGen_CreateImageGenerationRequest_ResolveImageGenerationRequested(FlowAction<string> initialPrompt)
        {
 
            var imgGenRequest = new DallEGenerationRequest(initialPrompt.Parameters);
            return ImageGenActions.ImageGenRequested(imgGenRequest);
        }

        public FlowActionBase OnImageGenerationRequested_RequestImageGen_ResolveImageGeneration(FlowAction<DallEGenerationRequest> imgGenAction)
        {
            var result = _dallERepo.GenerateImage(imgGenAction.Parameters);
            if (result != null)
            {
                return ImageGenActions.ImageGenSucceeded(result);
            }
            else
            {
                return ImageGenActions.ImageGenFailed("Error while generating image");
            }
        }

    }
}
