using ActionFlow.Models;
using ImageGenFlow.Models;
using OpenAIConnector.DallERepository.models;


namespace ImageGenFlow
{
    //Reducer
    public class ImageGenReducer : IFlowStateReducer<ImageGenStateEntity>
    {
        public ImageGenStateEntity InitialState => new ImageGenStateEntity();

        public List<IFlowReductionBase<ImageGenStateEntity>> Reductions => new List<IFlowReductionBase<ImageGenStateEntity>>
        {
            this.reduce(GeneratedImages_OnImageGenSucceeded_StoreGeneratedImages, ImageGenActions.ImageGenSucceeded()),
            this.reduce(ErrorMessage_OnImageGenFailed_WriteErrorMessage, ImageGenActions.ImageGenFailed()),

        };


        //Reducer Methods
        public ImageGenStateEntity GeneratedImages_OnImageGenSucceeded_StoreGeneratedImages(FlowAction<DallEGenerationResponse> imgGenResult,
            ImageGenStateEntity currentState)
        {
            if(imgGenResult.Parameters.data.Count > 0)
            {
                foreach(DallEImageData img in  imgGenResult.Parameters.data)
                {
                    currentState.generatedImages.Add(img);
                }
            }
            return currentState;
        }

        public ImageGenStateEntity ErrorMessage_OnImageGenFailed_WriteErrorMessage(FlowAction<string> imgGenFailed,
            ImageGenStateEntity currentState)
        {
            currentState.errorMessage = imgGenFailed.Parameters;
            return currentState;
        }

    }
}
