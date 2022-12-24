using health_tracking_system_mobile.Models.Results.Abstract;
using health_tracking_system_mobile.Models.Results.Doctors;
using health_tracking_system_mobile.Models.Results.HospitalAdministrators;
using health_tracking_system_mobile.Models.Results.PatientCaretakers;
using health_tracking_system_mobile.Models.Results.Patients;

namespace health_tracking_system_mobile.Models.Results.Users;

public class ProfileResult : BaseResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public DoctorResult? Doctor { get; set; }
    public PatientResult? Patient { get; set; }
    public PatientCaretakerResult? PatientCaretaker { get; set; }
    public HospitalAdministratorResult? HospitalAdministrator { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public string BirthDateShort => BirthDate.ToShortDateString();
    public bool IsHospitalAdmin => HospitalAdministrator != null;
    public bool IsDoctor => Doctor != null;
    public bool IsPatient => Patient != null;
    public bool IsCaretaker => PatientCaretaker != null;
    public bool IsAssignedToHospital => IsHospitalAdmin || IsDoctor || IsPatient;
    public bool HasDoctor => Patient != null && !string.IsNullOrWhiteSpace(Patient.DoctorId);
    public bool HasCaretaker => Patient != null && !string.IsNullOrWhiteSpace(Patient.PatientCaretakerId);

    public string GetHospitalId()
    {
        if(Doctor != null)
            return Doctor.HospitalId;

        if(Patient != null)
            return Patient.HospitalId;

        return HospitalAdministrator?.HospitalId;
    }

    public string GetPatientDoctorId()
    {
        return Patient?.DoctorId;
    }

    public string GetPatientCaretakerId() {
        return Patient?.PatientCaretakerId;
    }
}