using Microsoft.ML.OnnxRuntime.Tensors;

namespace SkinCancerDetectionApp.Services.ImageTransformService;

public interface IImageTransform
{
    public Tensor<float> Apply(Tensor<float> image);
}
