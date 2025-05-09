using DetectionService.Models;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService.Interfaces;

public interface IDetectionService
{
    public DetectionOutput RunInference(Tensor<float> image);
}