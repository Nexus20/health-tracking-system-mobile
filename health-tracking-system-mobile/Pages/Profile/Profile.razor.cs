using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.LineChart;
using health_tracking_system_mobile.Models.Results.Doctors;
using health_tracking_system_mobile.Models.Results.Hospitals;
using health_tracking_system_mobile.Models.Results.PatientCaretakers;
using health_tracking_system_mobile.Models.Results.Patients;
using health_tracking_system_mobile.Services;
using Microsoft.AspNetCore.Components;

namespace health_tracking_system_mobile.Pages.Profile;

public partial class Profile {

    private LineConfig _lineChartConfig;
    private Chart _lineChart;

    [Inject] private HospitalService HospitalService { get; set; }
    [Inject] private DoctorService DoctorService { get; set; }
    [Inject] private HealthMeasurementsService HealthMeasurementsService { get; set; }
    [Inject] private PatientCaretakerService PatientCaretakerService { get; set; }

    public HospitalResult UserHospital { get; set; }
    public DoctorResult UserDoctor { get; set; }
    public PatientCaretakerResult UserCaretaker { get; set; }
    public List<PatientResult> DoctorPatients { get; set; }
    public List<PatientResult> CaretakerPatients { get; set; }

    public bool HospitalDataLoaded { get; private set; }
    public bool UserDoctorDataLoaded { get; private set; }
    public bool UserCaretakerDataLoaded { get; private set; }
    public bool DoctorPatientsDataLoaded { get; private set; }
    public bool CaretakerPatientsDataLoaded { get; private set; }

    public int CurrentHeartRate { get; set; }

    private int _ecgDataCount;

    protected override async Task OnInitializedAsync()
    {
        await StartLoadingHealthMeasurements();
        await LoadUserHospitalAsync();
        await LoadUserDoctorAsync();
        await LoadUserCaretakerAsync();
        await LoadDoctorPatientsAsync();

        if (CurrentUser.IsCaretaker)
        {
            await LoadCaretakerPatientsAsync();
        }

        await base.OnInitializedAsync();
    }

    private async Task LoadCaretakerPatientsAsync() {
        CaretakerPatients = await PatientCaretakerService.GetCaretakerPatientsByIdAsync(CurrentUser.CaretakerId);
        CaretakerPatientsDataLoaded = true;
    }

    private async Task LoadDoctorPatientsAsync() {

        if (!CurrentUser.IsDoctor)
            return;

        DoctorPatients = await DoctorService.GetDoctorPatientsByIdAsync(CurrentUser.DoctorId);
        DoctorPatientsDataLoaded = true;
    }

    private async Task LoadUserHospitalAsync() {

        if (!CurrentUser.IsAssignedToHospital)
            return;

        var hospitalId = CurrentUser.GetHospitalId();
        UserHospital = await HospitalService.GetHospitalByIdAsync(hospitalId);
        HospitalDataLoaded = true;
    }

    private async Task LoadUserDoctorAsync() {

        if (!CurrentUser.IsPatient || !CurrentUser.HasDoctor)
            return;

        var doctorId = CurrentUser.GetPatientDoctorId();
        UserDoctor = await DoctorService.GetDoctorByIdAsync(doctorId);
        UserDoctorDataLoaded = true;
    }

    private async Task LoadUserCaretakerAsync() {

        if (!CurrentUser.IsPatient || !CurrentUser.HasCaretaker)
            return;

        var caretakerId = CurrentUser.GetPatientCaretakerId();
        UserCaretaker = await PatientCaretakerService.GetPatientCaretakerByIdAsync(caretakerId);
        UserCaretakerDataLoaded = true;
    }

    private async Task StartLoadingHealthMeasurements()
    {
        InitializeEcgChart();
        await HealthMeasurementsService.ConnectToHubAsync();
        await HealthMeasurementsService.StartReceivingEcgData(async ecgPoint =>
        {
            //Console.WriteLine($"Ecg point received: ({ecgPoint.X}, {ecgPoint.Y})");

            if (_ecgDataCount > 100)
            {
                _ecgDataCount--;
                _lineChartConfig.Data.Labels.RemoveAt(0);
                (_lineChartConfig.Data.Datasets.ElementAt(0) as LineDataset<int>)!.RemoveAt(0);
            }

            _ecgDataCount++;
            _lineChartConfig.Data.Labels.Add(ecgPoint.X.ToString());
            (_lineChartConfig.Data.Datasets.ElementAt(0) as LineDataset<int>)!.Add(ecgPoint.Y);
            await _lineChart.Update();

        });
        await HealthMeasurementsService.StartReceivingHeartRateData(async data =>
        {
            //Console.WriteLine($"Heart rate received: ({data.HeartRate})");
            CurrentHeartRate = data.HeartRate;
            await InvokeAsync(StateHasChanged);
        });
    }

    private void InitializeEcgChart() {
        _lineChartConfig = new LineConfig() {
            Options = new LineOptions() {
                Responsive = true,
                Title = new OptionsTitle {
                    Display = true,
                    Text = "ECG"
                }
            }
        };

        var lineDataset = new LineDataset<int>() {
            Label = "ECG",
            Fill = false,
            LineTension = 0.5,
            BorderColor = "black"
        };

        _lineChartConfig.Data.Datasets.Add(lineDataset);
    }

    public override async ValueTask DisposeAsync()
    {
        await HealthMeasurementsService.DisconnectFromHubAsync();
        await base.DisposeAsync();
    }
}