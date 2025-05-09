using DetectionService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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

		builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		builder.Services.Configure<DetectionServiceSettings>(
			builder.Configuration.GetSection("DetectionServiceSettings"));
		builder.Services.AddSingleton<IDetectionService, LocalDetectionService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
