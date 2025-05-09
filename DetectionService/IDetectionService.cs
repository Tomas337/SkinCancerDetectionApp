using Microsoft.ML.OnnxRuntime;

namespace DetectionService;

public interface IDetectionService
{
    public DetectionOutput RunInference(byte[] imageArray);
}