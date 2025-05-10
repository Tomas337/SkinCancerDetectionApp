using DetectionService;
using DetectionService.Interfaces;
using DetectionService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkinCancerDetectionApp.Services.ImageTransformService;
using SkinCancerDetectionApp.Services.ImageTransformService.Interfaces;

namespace SkinCancerDetectionApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;

        builder.Configuration.AddJsonStream(stream);
		builder.Services.Configure<DetectionSettings>(
			builder.Configuration.GetSection("DetectionServiceSettings"));
		builder.Services.AddSingleton<IDetectionService, LocalDetectionService>();
		builder.Services.AddScoped<IImageTransformService, ImageTransformService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
