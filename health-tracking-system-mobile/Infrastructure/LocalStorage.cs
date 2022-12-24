namespace health_tracking_system_mobile.Infrastructure;

public class LocalStorage
{
    private readonly Dictionary<string, object> _dictionary;

    public object this[string key] => _dictionary[key];

    public LocalStorage()
    {
        _dictionary = new Dictionary<string, object>();
    }

    public TResult GetValue<TResult>(string key) where TResult : class
    {
        return _dictionary[key] as TResult;
    }

    public bool Contains(string key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void Add(string key, object value)
    {
        _dictionary.Add(key, value);
    }

    public void AddOrUpdate(string key, object value)
    {
        if (Contains(key))
        {
            Remove(key);
        }

        Add(key, value);
    }

    public void Remove(string key)
    {
        _dictionary.Remove(key);
    }
}