using ChartJs.Blazor.LineChart;
using ChartJs.Blazor;
using health_tracking_system_mobile.Models.Results.Doctors;
using health_tracking_system_mobile.Models.Results.Hospitals;
using health_tracking_system_mobile.Models.Results.PatientCaretakers;
using health_tracking_system_mobile.Models.Results.Patients;
using health_tracking_system_mobile.Services;
using Microsoft.AspNetCore.Components;
using ChartJs.Blazor.Common;

namespace health_tracking_system_mobile.Pages.PatientView;

public partial class PatientView
{
    [Parameter]
    public string PatientId { get; set; }

    private LineConfig _lineChartConfig;
    private Chart _lineChart;

    [Inject] private HospitalService HospitalService { get; set; }
    [Inject] private DoctorService DoctorService { get; set; }
    [Inject] private PatientService PatientService { get; set; }
    [Inject] private HealthMeasurementsService HealthMeasurementsService { get; set; }
    [Inject] private PatientCaretakerService PatientCaretakerService { get; set; }

    public PatientResult Patient { get; set; }
    public HospitalResult PatientHospital { get; set; }
    public DoctorResult PatientDoctor { get; set; }
    public PatientCaretakerResult PatientCaretaker { get; set; }

    public bool PatientDataLoaded { get; private set; }
    public bool PatientHospitalDataLoaded { get; private set; }
    public bool PatientDoctorDataLoaded { get; private set; }
    public bool PatientCaretakerDataLoaded { get; private set; }

    public int CurrentHeartRate { get; set; }
    private int _ecgDataCount;

    protected override async Task OnInitializedAsync()
    {
        await LoadPatientAsync();
        await LoadPatientHospitalAsync();

        if (CurrentUser.IsHospitalAdmin || CurrentUser.IsCaretaker)
        {
            await LoadPatientDoctorAsync();
        }

        if (CurrentUser.IsHospitalAdmin || CurrentUser.IsDoctor)
        {
            await LoadPatientCaretakerAsync();
        }

        if (CurrentUser.IsDoctor || CurrentUser.IsCaretaker)
        {
            await StartLoadingHealthMeasurements();
        }

        await base.OnInitializedAsync();
    }

    private async Task LoadPatientCaretakerAsync()
    {
        if (string.IsNullOrWhiteSpace(Patient.PatientCaretakerId))
            return;

        PatientCaretaker = await PatientCaretakerService.GetPatientCaretakerByIdAsync(Patient.PatientCaretakerId);
        PatientCaretakerDataLoaded = true;
    }

    private async Task LoadPatientDoctorAsync()
    {
        if (string.IsNullOrWhiteSpace(Patient.DoctorId))
            return;

        PatientDoctor = await DoctorService.GetDoctorByIdAsync(Patient.DoctorId);
        PatientDoctorDataLoaded = true;
    }

    private async Task LoadPatientAsync()
    {
        Patient = await PatientService.GetPatientByIdAsync(PatientId);
        PatientDataLoaded = true;
    }

    private async Task LoadPatientHospitalAsync() {
        PatientHospital = await HospitalService.GetHospitalByIdAsync(Patient.HospitalId);
        PatientHospitalDataLoaded = true;
    }

    private async Task StartLoadingHealthMeasurements() {
        InitializeEcgChart();
        await HealthMeasurementsService.ConnectToHubAsync();
        await HealthMeasurementsService.StartReceivingEcgData(async ecgPoint => {
            //Console.WriteLine($"Ecg point received: ({ecgPoint.X}, {ecgPoint.Y})");

            if (_ecgDataCount > 100) {
                _ecgDataCount--;
                _lineChartConfig.Data.Labels.RemoveAt(0);
                (_lineChartConfig.Data.Datasets.ElementAt(0) as LineDataset<int>)!.RemoveAt(0);
            }

            _ecgDataCount++;
            _lineChartConfig.Data.Labels.Add(ecgPoint.X.ToString());
            (_lineChartConfig.Data.Datasets.ElementAt(0) as LineDataset<int>)!.Add(ecgPoint.Y);
            await _lineChart.Update();

        });
        await HealthMeasurementsService.StartReceivingHeartRateData(async data => {
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
}