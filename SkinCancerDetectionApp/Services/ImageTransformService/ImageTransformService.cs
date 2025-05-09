using System.Drawing;
using System.Drawing.Imaging;
using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime.Tensors;
using SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;
using Size = System.Drawing.Size;

namespace SkinCancerDetectionApp.Services.ImageTransformService;

public class ImageTransformService : IImageTransformService
{
    private class FasterRcnnMobileNet320Transform : IImageTransform
    {
        public Tensor<float> Apply(Tensor<float> image)
        {
            // TODO: perform resizing to (320, 320)
            Tensor<float> transformedImage = image;
            return transformedImage;
        }
    }

    private class NoTransform : IImageTransform
    {
        public Tensor<float> Apply(Tensor<float> image)
        {
            return image;
        }
    }

    public IImageTransform GetImageTransform(IOptions<DetectionSettings> settings)
    {
        switch (settings.Value.Transform)
        {
            case "FasterRcnnMobileNet320":
                return new FasterRcnnMobileNet320Transform();
            case "None":
                return new NoTransform();
            default:
                throw new ArgumentException($"There is no image transform class associated with the key \"{settings.Value.Transform}\".");
        }
    }

    /// <summary>
    /// Converts a JPEG image stored in a byte array to an ONNX tensor.
    /// </summary>
    /// <param name="jpegArray">The JPEG image as a byte array.</param>
    /// <returns>
    /// Tensor of shape [1, 3, W, H].
    /// </returns>
    /// <remarks>
    /// Assumes the JPEG image has already been resized to the target dimensions.
    /// </remarks>
    public unsafe Tensor<float> JpegArrayToTensor(byte[] jpegArray)
    {
        using var ms = new MemoryStream(jpegArray);
        using var bitmap = new Bitmap(ms);

        var tensor = new DenseTensor<float>(new[] { 1, 3, bitmap.Width, bitmap.Height });

        BitmapData bmd = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

        for (int y = 0; y < bmd.Height; y++)
        {
            // row is a pointer to a full row of data with each of its colors
            byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);

            for (int x = 0; x < bmd.Width; x++)
            {
                for (int c = 0; c < 3; c++)
                {
                    // note the order of BGR
                    tensor[0, c, y, x] = row[x*3 + (3-c-1)] / (float)255.0;
                }
            }
        }
        bitmap.UnlockBits(bmd);

        return tensor;
    }
}