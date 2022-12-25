using Microsoft.Extensions.Logging;
using health_tracking_system_mobile.Data;
using health_tracking_system_mobile.Infrastructure;
using health_tracking_system_mobile.Services;
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

		var connectionOptions = new ConnectionOptions("https://192.168.0.111:7088");
		builder.Services.AddSingleton(connectionOptions);


        var options = new RestClientOptions() {
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };
        var restClient = new RestClient(options);
        builder.Services.AddSingleton(sp => restClient);
		builder.Services.AddSingleton<LocalStorage>();
		builder.Services.AddSingleton<WeatherForecastService>();
        var translateService = new TranslateService();
        translateService.LoadFromResourcesAsync().GetAwaiter().GetResult();
        builder.Services.AddSingleton(translateService);
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<HospitalService>();
        builder.Services.AddSingleton<DoctorService>();
        builder.Services.AddSingleton<PatientService>();
        builder.Services.AddSingleton<PatientCaretakerService>();
        builder.Services.AddSingleton<HealthMeasurementsService>();

        return builder.Build();
	}
}
