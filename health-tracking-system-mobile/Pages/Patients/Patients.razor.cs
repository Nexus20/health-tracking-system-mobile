using health_tracking_system_mobile.Models.Results.Patients;
using health_tracking_system_mobile.Services;
using Microsoft.AspNetCore.Components;

namespace health_tracking_system_mobile.Pages.Patients;

public partial class Patients
{
    public bool PatientsDataLoaded { get; private set; }
    public List<PatientResult> PatientsData { get; set; }

    [Inject] private HospitalService HospitalService { get; set; }
    [Inject] private DoctorService DoctorService { get; set; }
    [Inject] private PatientCaretakerService PatientCaretakerService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadPatientsAsync();
        await base.OnInitializedAsync();
    }

    private async Task LoadPatientsAsync()
    {
        if (CurrentUser.IsHospitalAdmin) {
            await LoadHospitalPatientsAsync();
        }

        if (CurrentUser.IsDoctor) {
            await LoadDoctorPatientsAsync();
        }

        if (CurrentUser.IsCaretaker) {
            await LoadCaretakerPatientsAsync();
        }

        PatientsDataLoaded = true;
    }

    private async Task LoadCaretakerPatientsAsync()
    {
        PatientsData = await PatientCaretakerService.GetCaretakerPatientsByIdAsync(CurrentUser.CaretakerId);
    }

    private async Task LoadDoctorPatientsAsync()
    {
        PatientsData = await DoctorService.GetDoctorPatientsByIdAsync(CurrentUser.DoctorId);
    }

    private async Task LoadHospitalPatientsAsync()
    {
        PatientsData = await HospitalService.GetHospitalPatientsByIdAsync(CurrentUser.GetHospitalId());
    }
}