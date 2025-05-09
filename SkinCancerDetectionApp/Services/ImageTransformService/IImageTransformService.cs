using DetectionService;
using Microsoft.Extensions.Options;

namespace SkinCancerDetectionApp.Services.ImageTransformService;

public interface IImageTransformService
{
    public IImageTransform GetImageTransform(IOptions<DetectionServiceSettings> settings);
}
