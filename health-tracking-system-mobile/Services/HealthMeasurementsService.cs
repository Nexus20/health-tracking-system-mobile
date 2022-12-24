using health_tracking_system_mobile.Infrastructure;
using health_tracking_system_mobile.Models.Results.HealthMeasurements;
using health_tracking_system_mobile.Services.Abstract;
using Microsoft.AspNetCore.SignalR.Client;
using RestSharp;

namespace health_tracking_system_mobile.Services;

public class HealthMeasurementsService : BaseHttpService
{
    private readonly HubConnection _hubConnection;
    private readonly UserService _userService;
    private readonly LocalStorage _localStorage;
    private bool _isConnectedToHub;

    public HealthMeasurementsService(RestClient restClient, ConnectionOptions connectionOptions, UserService userService, LocalStorage localStorage) : base(restClient, connectionOptions)
    {
        _userService = userService;
        _localStorage = localStorage;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{ApiUrl}/health-measurements", (opts) => {
                opts.HttpMessageHandlerFactory = (message) => {
                    if (message is HttpClientHandler clientHandler)
                        // always verify the SSL certificate
                        clientHandler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, sslPolicyErrors) => true;
                    return message;
                };
            })
            .Build();
    }

    public async Task ConnectToHubAsync() {

        if (_isConnectedToHub)
            return;

        try {
            await _hubConnection.StartAsync();
            Console.WriteLine("Connected to hub");

            _isConnectedToHub = true;
        }
        catch (Exception ex) {
            Console.WriteLine($"Hub connection error: {ex.Message}");
        }
    }

    public async Task DisconnectFromHubAsync() {

        if (!_isConnectedToHub) return;

        await _hubConnection.StopAsync();
        _isConnectedToHub = false;
        Console.WriteLine("Disconnected from hub");
    }

    public async Task StartReceivingEcgData(Func<EcgPoint, Task> handler) {
        _hubConnection.On<EcgPoint>("TransferEcgData", handler);
        await SendRequestToStartReceivingEcgData();
    }

    public async Task StartReceivingHeartRateData(Func<HeartRateModel, Task> handler) {
        _hubConnection.On<HeartRateModel>("TransferHeartRateData", handler);
        await SendRequestToStartReceivingHeartRateData();
    }

    private async Task SendRequestToStartReceivingEcgData() {
        var url = $"{ApiUrl}/api/chart/getEcg";
        var request = new RestRequest(new Uri(url));

        if (_userService.IsAuthenticated)
            request = request.AddHeader("Authorization", "Bearer " + _localStorage[LocalStorageKeys.AuthToken]);

        var response = await RestClient.ExecuteAsync(request);

        Console.WriteLine(response.Content);

        if (!response.IsSuccessful) {
            throw new ApplicationException();
        }
    }

    private async Task SendRequestToStartReceivingHeartRateData() {
        var url = $"{ApiUrl}/api/chart/getHeartRate";
        var request = new RestRequest(new Uri(url));

        if (_userService.IsAuthenticated)
            request = request.AddHeader("Authorization", "Bearer " + _localStorage[LocalStorageKeys.AuthToken]);

        var response = await RestClient.ExecuteAsync(request);

        Console.WriteLine(response.Content);

        if (!response.IsSuccessful) {
            throw new ApplicationException();
        }
    }
}