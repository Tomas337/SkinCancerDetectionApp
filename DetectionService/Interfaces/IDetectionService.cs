using DetectionService.Models;

namespace DetectionService.Interfaces;

public interface IDetectionService
{
    public DetectionOutput RunInference(byte[] imageArray);
}