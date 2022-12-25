using health_tracking_system_mobile.Models.Requests.Auth;

namespace health_tracking_system_mobile.Pages.Auth
{
    public partial class Login
    {
        public LoginRequest LoginModel { get; set; } = new LoginRequest()
        {
            // Email = "test13hadmin@ht.com", // hospital admin
            // Email = "test13p1@ht.com", // patient without caretaker
            // Email = "test777patient@ht.com", // patient with caretaker
            // Email = "test777doctor@ht.com", // doctor
            Email = "test777pcare@ht.com", // caretaker
            Password = "_QGrXyvcmTD4aVQJ_",
        };

        public async Task OnSubmitAsync()
        {
            try
            {
                await UserService.LoginAsync(LoginModel);
                NavigationManager.NavigateTo("/profile", true);
            }
            catch (Exception ex)
            {
                //AddError(ex);
            }
        }
    }
}
