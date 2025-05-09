using Microsoft.ML.OnnxRuntime.Tensors;

namespace DetectionService.Models;

public sealed class DetectionOutput
{
    public Tensor<float> BBox;
    public Tensor<int> Label;
    public Tensor<float> Score;

    public DetectionOutput(Tensor<float> bbox, Tensor<int> label, Tensor<float> score)
    {
        BBox = bbox;
        Label = label;
        Score = score;
    }
}