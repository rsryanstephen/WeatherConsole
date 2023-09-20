using Domain.Models.WeatherStack;

namespace Domain.Interfaces;

public interface IResponseProcessor
{
    Task ProcessApiResponse(ApiResponse response);
}