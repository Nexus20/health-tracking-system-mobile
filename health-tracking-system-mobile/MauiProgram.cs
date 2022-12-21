using Microsoft.Extensions.Logging;
using health_tracking_system_mobile.Data;
using health_tracking_system_mobile.Infrastructure;
using RestSharp;

namespace health_tracking_system_mobile;

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

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var connectionOptions = new ConnectionOptions("https://192.168.0.102:7292");
		builder.Services.AddSingleton(connectionOptions);
		builder.Services.AddSingleton<RestClient>();
		builder.Services.AddSingleton<WeatherForecastService>();

		return builder.Build();
	}
}
