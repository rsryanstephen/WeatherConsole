using Domain.Models.WeatherStack;

namespace WeatherApiService.Http;

public interface IHttpClientWrapper
{
    Task<T> GetFromHttpResponseAsync<T>(string requestUri);
}