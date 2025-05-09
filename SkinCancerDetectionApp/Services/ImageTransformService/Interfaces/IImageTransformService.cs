using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;

public interface IImageTransformService
{
    public IImageTransform GetImageTransform(IOptions<DetectionSettings> settings);
    public Tensor<float> JpegArrayToTensor(byte[] jpegArray, bool grayscale = false);
}
