using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApiService.Http;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _client;

    public HttpClientWrapper(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.Create();
    }

    public async Task<T> GetFromHttpResponseAsync<T>(string requestUri)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await _client.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var msg = $"{response.StatusCode} status returned for 'GetAsync'." +
                      $"{Environment.NewLine}requestUri: '{requestUri}'";
            
            throw new HttpRequestException(msg);
        }

        var json = await response.Content.ReadAsStringAsync();
        
        CheckForFailedResponseJson(json, requestUri);

        return JsonConvert.DeserializeObject<T>(json) 
               ?? throw new Exception("HttpResponse.Content.ReadAsStringAsync() returned null");
    }
    
    private static void CheckForFailedResponseJson(string? json, string requestUri)
    {
        if (json == null)
        {
            var msg = 
                "Null response when using HttpClient.GetFromHttpResponseAsync." +
                $"{Environment.NewLine}requestUri: '{requestUri}'";
            
            throw new Exception(msg);
        }
        
        var jToken = JToken.Parse(json);

        if (JTokenHasError(jToken))
        {
            var msg = $"Error response returned for 'GetAsync'." +
                      $"{Environment.NewLine}requestUri: '{requestUri}'" +
                      $"{Environment.NewLine}json: '{json}'";
            
            throw new KeyNotFoundException(msg);
        }
    }
    
    private static bool JTokenHasError(JToken jToken)
    {
        return jToken["error"] != null || 
               (jToken["success"] != null && jToken["success"]?.Value<bool>() == false);
    }
}