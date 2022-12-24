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
}