using health_tracking_system_mobile.Infrastructure;
using health_tracking_system_mobile.Models.Results.PatientCaretakers;
using health_tracking_system_mobile.Services.Abstract;
using Newtonsoft.Json;
using RestSharp;

namespace health_tracking_system_mobile.Services;

public class PatientCaretakerService : BaseHttpService {

    private readonly LocalStorage _localStorage;
    private readonly UserService _userService;

    public PatientCaretakerService(RestClient restClient, ConnectionOptions connectionOptions, LocalStorage localStorage, UserService userService) : base(restClient, connectionOptions) {
        _localStorage = localStorage;
        _userService = userService;
    }

    public async Task<PatientCaretakerResult> GetPatientCaretakerByIdAsync(string patientCaretakerId) {
        var url = $"{ApiUrl}/api/patientCaretaker/{patientCaretakerId}";
        var request = new RestRequest(new Uri(url));

        if (_userService.IsAuthenticated)
            request = request.AddHeader("Authorization", "Bearer " + _localStorage[LocalStorageKeys.AuthToken]);

        var response = await RestClient.ExecuteAsync(request);

        Console.WriteLine(response.Content);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content)) {
            throw new ApplicationException();
        }

        return JsonConvert.DeserializeObject<PatientCaretakerResult>(response.Content);
    }
}