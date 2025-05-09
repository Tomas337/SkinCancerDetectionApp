using Microsoft.ML.OnnxRuntime.Tensors;

namespace SkinCancerDetectionApp.Services;

public interface IImageTransform
{
    public Tensor<float> Apply(Tensor<float> image);
}
