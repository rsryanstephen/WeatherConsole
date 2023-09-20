using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WeatherApiService.Http;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _client;

    public HttpClientWrapper(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.Create();
    }
    
    public async Task<T> GetAsync<T>(string requestUri)
    {
        await using var responseStream = await _client.GetStreamAsync(requestUri);
        var responseObject = await JsonSerializer.DeserializeAsync<T>(responseStream);

        if (responseObject == null)
        {
            var msg = 
                "Null response when using HttpClient.GetStreamAsync." +
                $"{Environment.NewLine}requestUri: '{requestUri}'";
            
            throw new Exception(msg);
        }

        return responseObject;
    }

    public async Task<T> GetFromHttpResponseAsync<T>(string requestUri)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await _client.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var msg = $"{response.StatusCode} status returned for 'GetAsync'." +
                      $"{Environment.NewLine}requestUri: '{requestUri}'";
            
            throw new ApplicationException(msg);
        }

        var json = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(json) 
               ?? throw new Exception("HttpResponse.Content.ReadAsStringAsync() returned null");
    }
}