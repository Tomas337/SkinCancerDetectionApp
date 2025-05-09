using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;

public interface IImageTransformService
{
    public IImageTransform GetImageTransform();
    public Tensor<float> JpegArrayToTensor(byte[] jpegArray, bool grayscale = false);
}
