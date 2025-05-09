using System.Threading.Channels;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService;

public class LocalDetectionService : IDetectionService
{
    private readonly InferenceSession _session;

    public LocalDetectionService(IOptions<DetectionServiceSettings> settings)
    {
        _session = new InferenceSession(settings.Value.LocalModelPath);
    }

    public DetectionOutput RunInference(byte[] imageArray)
    {
        return new DetectionOutput();
    }
}