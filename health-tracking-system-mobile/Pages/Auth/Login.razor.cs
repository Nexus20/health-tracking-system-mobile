﻿using health_tracking_system_mobile.Models.Requests.Auth;

namespace health_tracking_system_mobile.Pages.Auth
{
    public partial class Login
    {
        public LoginRequest LoginModel { get; set; } = new LoginRequest()
        {
            Email = "test13hadmin@ht.com",
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