using health_tracking_system_mobile.Models.Results.Abstract;
using health_tracking_system_mobile.Models.Results.Patients;

namespace health_tracking_system_mobile.Models.Results.PatientCaretakers;

public class PatientCaretakerResult : BaseResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public List<PatientResult>? Patients { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}