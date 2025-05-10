using DetectionService.Models;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService.Interfaces;

public interface IDetectionService
{
    public Task<DetectionOutput?> RunInference(Tensor<float> image);
}