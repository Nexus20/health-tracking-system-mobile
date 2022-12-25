using health_tracking_system_mobile.Infrastructure;
using health_tracking_system_mobile.Models.Results.Users;
using health_tracking_system_mobile.Services;
using Microsoft.AspNetCore.Components;

namespace health_tracking_system_mobile.Components.Common
{
    public class BaseComponent : ComponentBase, IDisposable, IAsyncDisposable
    {
        private readonly List<string> _errors = new List<string>();
        private readonly List<string> _messages = new List<string>();

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected UserService UserService { get; set; }
        //[Inject] protected AnimalService AnimalService { get; set; }
        //[Inject] protected PhotoService PhotoService { get; set; }
        [Inject] protected ConnectionOptions ConnectionOptions { get; set; }
        [Inject] protected LocalStorage LocalStorage { get; set; }
        [Inject] public TranslateService Translate { get; set; }

        //public bool IsAdmin => AuthService?.IsAdmin ?? false;
        //public bool IsAuthorized => AuthService?.IsAuthorized ?? false;
        public ProfileResult CurrentUser => UserService?.CurrentUser ?? null;

        public List<string> Errors => _errors;
        public List<string> Messages => _messages;

        protected override void OnInitialized()
        {
            Translate.LanguageChanged += LanguageChanged;
        }

        public void AddError(Exception ex)
        {
            _messages.Clear();
            _errors.Clear();
            _errors.Add(ex.Message);
            StateHasChanged();
        }

        public void ClearError()
        {
            _errors.Clear();
            StateHasChanged();
        }

        public void AddMessage(string message)
        {
            _errors.Clear();
            _messages.Clear();
            _messages.Add(message);
            StateHasChanged();
        }

        public void ClearMessage()
        {
            _messages.Clear();
            StateHasChanged();
        }

        public virtual void Dispose()
        {
            Translate.LanguageChanged -= LanguageChanged;
            ClearError();
            ClearMessage();
        }

        public virtual ValueTask DisposeAsync() {
            Translate.LanguageChanged -= LanguageChanged;
            ClearError();
            ClearMessage();
            return ValueTask.CompletedTask;
        }

        //protected GenericService<TEntity> GetGenericService<TEntity>() where TEntity : BaseModel
        //{
        //    return new GenericService<TEntity>(ConnectionOptions, LocalStorage);
        //}

        private void LanguageChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
