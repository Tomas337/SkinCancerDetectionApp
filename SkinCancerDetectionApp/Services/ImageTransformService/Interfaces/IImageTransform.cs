using Microsoft.ML.OnnxRuntime.Tensors;

namespace SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;

public interface IImageTransform
{
    public Tensor<float> Apply(Tensor<float> image);
}
