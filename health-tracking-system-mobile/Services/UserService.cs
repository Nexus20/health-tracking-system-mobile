using health_tracking_system_mobile.Infrastructure;
using health_tracking_system_mobile.Models.Requests.Auth;
using health_tracking_system_mobile.Models.Results.Auth;
using health_tracking_system_mobile.Models.Results.Users;
using Newtonsoft.Json;
using RestSharp;

namespace health_tracking_system_mobile.Services;

public class UserService
{
    public ProfileResult CurrentUser { get; private set; }
    public bool IsAuthenticated { get; private set; }

    private readonly LocalStorage _localStorage;
    private readonly RestClient _restClient;
    private readonly string _apiUrl;

    public UserService(RestClient restClient, ConnectionOptions connectionOptions, LocalStorage localStorage) {
        _restClient = restClient;
        _localStorage = localStorage;
        _apiUrl = connectionOptions.BaseUrl;
    }

    public async Task LoginAsync(LoginRequest requestBody) {
        var url = _apiUrl + "/api/auth/login";
        var request = new RestRequest(new Uri(url), Method.Post);
        var json = JsonConvert.SerializeObject(requestBody);
        request.AddStringBody(json, "application/json");
        var response = await _restClient.ExecuteAsync(request);

        Console.WriteLine(response.Content);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content)) {
            throw new ApplicationException();
        }

        var authResult = JsonConvert.DeserializeObject<LoginResult>(response.Content);
        _localStorage.AddOrUpdate(LocalStorageKeys.AuthToken, authResult.Token);
        var currentUser = await GetCurrentUserProfileAsync();
        _localStorage.AddOrUpdate(LocalStorageKeys.Profile, currentUser);
        CurrentUser = currentUser;
        IsAuthenticated = true;
    }

    public async Task<ProfileResult> GetCurrentUserProfileAsync()
    {
        var url = _apiUrl + "/api/users/profile";
        var request = new RestRequest(new Uri(url)).AddHeader("Authorization", "Bearer " + _localStorage[LocalStorageKeys.AuthToken]); ;
        var response = await _restClient.ExecuteAsync(request);

        Console.WriteLine(response.Content);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content)) {
            throw new ApplicationException();
        }

        return JsonConvert.DeserializeObject<ProfileResult>(response.Content);
    }

    public void Logout()
    {
        CurrentUser = null;
        IsAuthenticated = false;
        _localStorage.Remove(LocalStorageKeys.AuthToken);
        _localStorage.Remove(LocalStorageKeys.Profile);
    }
}