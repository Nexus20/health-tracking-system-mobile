namespace health_tracking_system_mobile.Shared
{
    public partial class NavMenu
    {
        private bool _collapseNavMenu = true;
        private string NavMenuCssClass => _collapseNavMenu ? "collapse" : string.Empty;

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
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
