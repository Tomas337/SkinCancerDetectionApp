using DetectionService;
using Microsoft.Extensions.Options;

namespace SkinCancerDetectionApp.Services;

public interface IImageTransformService
{
    public IImageTransform GetImageTransform(IOptions<DetectionServiceSettings> settings);
}
