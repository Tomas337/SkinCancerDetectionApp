using System.Diagnostics;
using DetectionService.Interfaces;
using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService;

public class LocalDetectionService : IDetectionService
{
    private InferenceSession? _session;
    private IOptions<DetectionSettings> _settings;

    public LocalDetectionService(IOptions<DetectionSettings> settings)
    {
        _settings = settings;
    }

    public async Task<DetectionOutput?> RunInference(Tensor<float> image)
    {
        if (_session is null)
        {
            string onnxModelPath = await FileOperations.CopyToAppData(_settings.Value.LocalModelPath);
            _session = new InferenceSession(onnxModelPath);
        }

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