using Domain.Models.WeatherStack;

namespace Domain.Interfaces;

public interface IResponseProcessor
{
    Task<bool> ProcessApiResponse(string cityName, ApiResponse? response);
}