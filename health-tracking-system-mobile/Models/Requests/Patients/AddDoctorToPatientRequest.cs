using System.ComponentModel.DataAnnotations;

namespace health_tracking_system_mobile.Models.Requests.Patients;

public class AddDoctorToPatientRequest
{
    [Required] public string DoctorId { get; set; } = null!;
}