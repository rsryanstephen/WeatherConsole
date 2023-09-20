namespace WeatherApiService.Http;

public interface IHttpClientFactory
{
    HttpClient Create();
}