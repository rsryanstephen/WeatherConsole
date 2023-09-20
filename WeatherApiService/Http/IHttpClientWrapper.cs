using Domain.Models.WeatherStack;

namespace WeatherApiService.Http;

public interface IHttpClientWrapper
{
    Task<T> GetAsync<T>(string requestUri);
    Task<T> GetFromHttpResponseAsync<T>(string requestUri);
}