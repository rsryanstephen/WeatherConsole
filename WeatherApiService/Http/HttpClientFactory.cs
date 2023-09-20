using System.Net.Http.Headers;

namespace WeatherApiService.Http;

/// <summary>
/// Ideally this must be registered as a singleton so that
/// only one instance of the http client is created.
/// That way open http connections can be reused.
/// </summary>
public class HttpClientFactory : IHttpClientFactory
{
    private const int DEFAULT_TIMEOUT = 30;
    private readonly HttpClient _client;

    public HttpClientFactory()
    {
        // only one instance of http client so that we can reuse open http connections
        _client = CreateClient(DEFAULT_TIMEOUT);
    }

    public HttpClient Create()
    {
        return _client;
    }
    
    private static HttpClient CreateClient(int timeout)
    {
        var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(timeout)
        };

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }
}