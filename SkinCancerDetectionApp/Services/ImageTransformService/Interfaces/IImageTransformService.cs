using DetectionService;
using DetectionService.Models;
using Microsoft.Extensions.Options;

namespace SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;

public interface IImageTransformService
{
    public IImageTransform GetImageTransform(IOptions<DetectionSettings> settings);
}
