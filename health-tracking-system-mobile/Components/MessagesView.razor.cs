using Microsoft.AspNetCore.Components;

namespace health_tracking_system_mobile.Components
{
    public partial class MessagesView
    {
        [Parameter] public List<string> Errors { get; set; }
        [Parameter] public List<string> Messages { get; set; }

        public MessagesView()
        {
            Errors = new();
            Messages = new();
        }
    }
}
