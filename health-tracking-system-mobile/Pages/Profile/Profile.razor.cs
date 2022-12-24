using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.LineChart;
using health_tracking_system_mobile.Models.Results.Doctors;
using health_tracking_system_mobile.Models.Results.Hospitals;
using health_tracking_system_mobile.Services;
using Microsoft.AspNetCore.Components;

namespace health_tracking_system_mobile.Pages.Profile;

public partial class Profile {

    private LineConfig _lineChartConfig;
    private Chart _lineChart;

    [Inject] private HospitalService HospitalService { get; set; }
    [Inject] private DoctorService DoctorService { get; set; }
    [Inject] private HealthMeasurementsService HealthMeasurementsService { get; set; }

    public HospitalResult UserHospital { get; set; }
    public DoctorResult UserDoctor { get; set; }

    public bool HospitalDataLoaded { get; set; }
    public bool UserDoctorDataLoaded { get; set; }

    [Parameter] public int CurrentHeartRate { get; set; }

    private int _ecgDataCount = 0;

    protected override async Task OnInitializedAsync()
    {
        await StartLoadingHealthMeasurements();
        await LoadUserHospitalAsync();
        await LoadUserDoctorAsync();
        await base.OnInitializedAsync();
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

    public override void Dispose()
    {
        HealthMeasurementsService.DisconnectFromHubAsync().Wait();
        base.Dispose();
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
}