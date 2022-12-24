using health_tracking_system_mobile.Infrastructure;
using RestSharp;

namespace health_tracking_system_mobile.Services.Abstract;

public abstract class BaseHttpService
{
    protected readonly RestClient RestClient;
    protected readonly string ApiUrl;

    protected BaseHttpService(RestClient restClient, ConnectionOptions connectionOptions)
    {
        RestClient = restClient;
        ApiUrl = connectionOptions.BaseUrl;
    }
}