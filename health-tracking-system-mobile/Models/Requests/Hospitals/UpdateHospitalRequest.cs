using System.ComponentModel.DataAnnotations;

namespace health_tracking_system_mobile.Models.Requests.Hospitals;

public class UpdateHospitalRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
}