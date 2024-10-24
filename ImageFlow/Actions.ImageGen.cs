using ActionFlow.Models;
using OpenAIConnector.ChatGPTRepository.models;
using OpenAIConnector.DallERepository.models;

namespace ImageGenFlow
{
    public static class ImageGenActions
    {
        public static FlowAction<string> InitImageGen(string prompt = "") => new FlowAction<string> { Name = "InitialImageGen", Parameters = prompt };
        public static FlowAction<DallEGenerationRequest> ImageGenRequested(DallEGenerationRequest? request = null) => new FlowAction<DallEGenerationRequest> { Name = "ImageGenerationRequested", Parameters = request };
        public static FlowAction<DallEGenerationResponse> ImageGenSucceeded(DallEGenerationResponse? response = null) => new FlowAction<DallEGenerationResponse> { Name = "ImageGenerationSucceeded", Parameters = response };
        public static FlowAction<string> ImageGenFailed(string? response = null) => new FlowAction<string> { Name = "ImageGenerationFailed", Parameters = response };

        //public static FlowAction ImageGenCompleted() => new FlowAction { Name = "ImageGenCompleted" };
        //public static FlowAction<string> ImageGenNoOp(string source = "") => new FlowAction<string> { Name = "ImageGenNoOp", Parameters = source};

    }
}
