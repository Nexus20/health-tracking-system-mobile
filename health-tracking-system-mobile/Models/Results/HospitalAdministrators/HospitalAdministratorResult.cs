using health_tracking_system_mobile.Models.Results.Abstract;

namespace health_tracking_system_mobile.Models.Results.HospitalAdministrators;

public class HospitalAdministratorResult : BaseResult
{
    public string HospitalId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}