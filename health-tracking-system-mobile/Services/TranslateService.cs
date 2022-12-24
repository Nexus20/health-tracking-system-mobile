using Newtonsoft.Json.Linq;

namespace health_tracking_system_mobile.Services;

public class TranslateService
{
    private readonly string[] _availableLanguages = new string[] { "ua", "en" };
    private string _currentLanguage = "ua";
    private readonly Dictionary<string, Dictionary<string, string>> _translations;

    public event EventHandler LanguageChanged;

    public string[] AvailableLanguages => _availableLanguages;

    public string CurrentLanguage => _currentLanguage;

    public string this[string key]
    {
        get
        {
            if (_translations.ContainsKey(_currentLanguage) 
                && _translations[_currentLanguage].ContainsKey(key))
            {
                return _translations[_currentLanguage][key];
            }

            return key;
        }
    }

    public TranslateService()
    {
        _translations = new Dictionary<string, Dictionary<string, string>>();
    }

    public async Task LoadFromResourcesAsync()
    {
        try
        {
            foreach (var language in _availableLanguages)
            {
                if (await FileSystem.AppPackageFileExistsAsync($"{language}.json"))
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync($"{language}.json");
                    using var reader = new StreamReader(stream);
                    var translates = JObject.Parse(reader.ReadToEnd());
                    Dictionary<string, string> dictionary = new();

                    foreach (var record in translates)
                    {
                        if (!dictionary.ContainsKey(record.Key))
                        {
                            dictionary.Add(record.Key, record.Value.ToString());
                        }
                    }

                    _translations.Add(language, dictionary);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void ChangeLanguage(string newLanguage)
    {
        _currentLanguage = newLanguage;
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }
}