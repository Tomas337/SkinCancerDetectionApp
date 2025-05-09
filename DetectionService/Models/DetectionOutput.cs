using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService.Models;

public sealed class DetectionOutput
{
    public Tensor<float> Boxes;
    public Tensor<int> Labels;
    public Tensor<float> Scores;

    public DetectionOutput(Tensor<float> boxes, Tensor<int> labels, Tensor<float> scores)
    {
        Boxes = boxes;
        Labels = labels;
        Scores = scores;
    }
}