using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using DetectionService.Models;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntime.Tensors;
using SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;
using Image = System.Drawing.Image;

namespace SkinCancerDetectionApp.Services.ImageTransformService;

public class ImageTransformService : IImageTransformService
{
    private readonly IOptions<DetectionSettings> _settings;

    public ImageTransformService(IOptions<DetectionSettings> settings)
    {
        _settings = settings;
    }

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

    public IImageTransform GetImageTransform()
    {
        switch (_settings.Value.Transform)
        {
            case "FasterRcnnMobileNet320":
                return new FasterRcnnMobileNet320Transform();
            case "None":
                return new NoTransform();
            default:
                throw new ArgumentException($"There is no image transform class associated with the key \"{_settings.Value.Transform}\".");
        }
    }

    /// <summary>
    /// Converts a JPEG image stored in a byte array to an ONNX tensor.
    /// </summary>
    /// <param name="jpegArray">The JPEG image as a byte array.</param>
    /// <param name="grayscale">If true, the output tensor will have the shape [1, 1, W, H]; otherwise, it will have the shape [1, 3, W, H].</param>
    /// <returns>
    /// A tensor with the shape [1, C, W, H], where C is 1 for grayscale and 3 for color.
    /// </returns>
    /// <remarks>
    /// This method does not resize the image.
    /// This implementation uses marshalling and, therefore, may not be the fastest implementation.
    /// </remarks>
    public Tensor<float> JpegArrayToTensor(byte[] jpegArray, bool grayscale = false)
    {
        using var ms = new MemoryStream(jpegArray);
        using var bitmap = new Bitmap(ms);

        BitmapData bmd = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

        int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
        int stride = bmd.Stride;
        int width = bitmap.Width;
        int height = bitmap.Height;
        int channels = grayscale ? 1 : 3;

        byte[] pixelData = new byte[stride * height];
        Marshal.Copy(bmd.Scan0, pixelData, 0, pixelData.Length);
        bitmap.UnlockBits(bmd);

        var tensor = new DenseTensor<float>(new[] { 1, channels, width, height });

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * stride + x * bytesPerPixel;
                byte blue = pixelData[index];
                byte green = pixelData[index + 1];
                byte red = pixelData[index + 2];

                if (grayscale)
                {
                    float gray = (0.299f * red + 0.587f * green + 0.114f * blue) / 255f;
                    tensor[0, 0, y, x] = gray;
                }
                else
                {
                    tensor[0, 0, y, x] = red / 255f;
                    tensor[0, 1, y, x] = green / 255f;
                    tensor[0, 2, y, x] = blue / 255f;
                }
            }
        }
        bitmap.UnlockBits(bmd);

        return tensor;
    }
}