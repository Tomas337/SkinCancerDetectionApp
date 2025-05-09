using DetectionService;
using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime.Tensors;
using SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;

namespace SkinCancerDetectionApp.Services.ImageTransformService;

public class ImageTransformService : IImageTransformService
{
    private class FasterRcnnMobileNet320Transform : IImageTransform
    {
        public Tensor<float> Apply(Tensor<float> image)
        {
            // TODO: perform resizing to (320, 320)
            Tensor<float> transformedImage = image;
            return transformedImage;
        }
    }

    private class NoTransform : IImageTransform
    {
        public Tensor<float> Apply(Tensor<float> image)
        {
            return image;
        }
    }

    public IImageTransform GetImageTransform(IOptions<DetectionSettings> settings)
    {
        switch (settings.Value.Transform)
        {
            case "FasterRcnnMobileNet320":
                return new FasterRcnnMobileNet320Transform();
            case "None":
                return new NoTransform();
            default:
                throw new ArgumentException($"There is no image transform class associated with the key \"{settings.Value.Transform}\".");
        }
    }
}