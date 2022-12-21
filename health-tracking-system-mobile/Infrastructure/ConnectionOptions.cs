namespace health_tracking_system_mobile.Infrastructure;

public class ConnectionOptions
{
    public string BaseUrl { get; set; }

    public ConnectionOptions(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
}