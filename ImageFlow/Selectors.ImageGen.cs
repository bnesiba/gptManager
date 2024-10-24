using ActionFlow.Models;
using ImageGenFlow.Models;
using OpenAIConnector.DallERepository.models;


namespace ImageGenFlow
{
    public static class ImageGenSelectors
    {
        public static FlowDataSelector<ImageGenStateEntity, ImageGenStateEntity> GetImageGenResults = new FlowDataSelector<ImageGenStateEntity, ImageGenStateEntity>((stateData) => stateData);

        public static FlowDataSelector<ImageGenStateEntity, List<DallEImageData>> GetGeneratedImages = new FlowDataSelector<ImageGenStateEntity, List<DallEImageData>>((stateData) => stateData.generatedImages);
        public static FlowDataSelector<ImageGenStateEntity, string> GetErrorMessages = new FlowDataSelector<ImageGenStateEntity, string>((stateData) => stateData.errorMessage);

        
    }
}
