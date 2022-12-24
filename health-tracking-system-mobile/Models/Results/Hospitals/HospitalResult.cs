using health_tracking_system_mobile.Models.Results.Abstract;

namespace health_tracking_system_mobile.Models.Results.Hospitals;

public class HospitalResult : BaseResult
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int DoctorsCount { get; set; }
    public int PatientsCount { get; set; }
}