using Domain.Models.WeatherStack;

namespace Domain.Interfaces;

public interface IWeatherApiService
{
    Task<ApiResponse> GetCurrentWeather(string cityName);
}