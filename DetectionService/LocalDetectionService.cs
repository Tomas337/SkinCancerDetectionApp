using System.Diagnostics;
using System.Threading.Channels;
using DetectionService.Interfaces;
using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService;

public class LocalDetectionService : IDetectionService
{
    private readonly InferenceSession _session;

    public LocalDetectionService(IOptions<DetectionSettings> settings)
    {
        _session = new InferenceSession(settings.Value.LocalModelPath);
    }

    public DetectionOutput? RunInference(Tensor<float> image)
    {
        var inputName = _session.InputMetadata.Keys.First();
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(inputName, image)
        };

        var results = _session.Run(inputs);

        var output = results.FirstOrDefault();
        if (output != null)
        {
            var bbox = output.AsTensor<float>();
            var label = output.AsTensor<int>();
            var score = output.AsTensor<float>();
            Trace.WriteLine($"bbox: {bbox}, label: {label}, score: {score}");
            return new DetectionOutput(bbox, label, score);
        }
        return null;
    }
}