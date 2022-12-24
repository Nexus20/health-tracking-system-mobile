namespace health_tracking_system_mobile.Shared
{
    public partial class NavMenu
    {
        private bool collapseNavMenu = true;
        private string NavMenuCssClass => collapseNavMenu ? "collapse" : string.Empty;
        //private int? CurrentUserId => AuthService?.CurrentUser?.Id;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private Task ChangeLanguage(string language)
        {
            Translate.ChangeLanguage(language);
            return InvokeAsync(StateHasChanged);
        }

        private void Logout()
        {
            UserService.Logout();
            NavigationManager.NavigateTo("/login");
        }
    }
}
