
using OpenAIConnector.DallERepository.models;

namespace ImageGenFlow.Models
{
    public class ImageGenStateEntity
    {
        public string initialPrompt { get; set; }
        public List<DallEImageData> generatedImages {  get; set; }
        public string errorMessage { get; set; }


        public ImageGenStateEntity()
        {
            initialPrompt = string.Empty;
            generatedImages = new List<DallEImageData>();
            errorMessage = string.Empty;
        }

        public ImageGenStateEntity Copy()
        {
            var copy = new ImageGenStateEntity();
            copy.initialPrompt = initialPrompt;
            copy.generatedImages = generatedImages.ToList();
            copy.errorMessage = errorMessage;
            return copy;
        }

        public override string ToString()
        {
            return $"InitialPrompt: {initialPrompt} ImageUrlCount: {generatedImages.Count}";
        }
    }

}
