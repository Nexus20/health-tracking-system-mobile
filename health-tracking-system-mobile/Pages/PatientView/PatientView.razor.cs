using Microsoft.AspNetCore.Components;

namespace health_tracking_system_mobile.Pages.PatientView;

public partial class PatientView
{
    [Parameter]
    public string PatientId { get; set; }

    protected override Task OnInitializedAsync()
    {
        Console.WriteLine("PatientId: " + PatientId);
        return base.OnInitializedAsync();
    }
}