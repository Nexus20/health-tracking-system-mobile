using health_tracking_system_mobile.Infrastructure;
using health_tracking_system_mobile.Models.Results.Patients;
using health_tracking_system_mobile.Services.Abstract;
using Newtonsoft.Json;
using RestSharp;

namespace health_tracking_system_mobile.Services;

public class PatientService : BaseHttpService {

    private readonly LocalStorage _localStorage;
    private readonly UserService _userService;

    public PatientService(RestClient restClient, ConnectionOptions connectionOptions, LocalStorage localStorage, UserService userService) : base(restClient, connectionOptions) {
        _localStorage = localStorage;
        _userService = userService;
    }

    public async Task<PatientResult> GetPatientByIdAsync(string patientId) {
        var url = $"{ApiUrl}/api/patient/{patientId}";
        var request = new RestRequest(new Uri(url));

        if (_userService.IsAuthenticated)
            request = request.AddHeader("Authorization", "Bearer " + _localStorage[LocalStorageKeys.AuthToken]);

        var response = await RestClient.ExecuteAsync(request);

        Console.WriteLine(response.Content);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content)) {
            throw new ApplicationException();
        }

        return JsonConvert.DeserializeObject<PatientResult>(response.Content);
    }
}