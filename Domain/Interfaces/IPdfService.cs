using Domain.Models.WeatherStack;

namespace Domain.Interfaces;

public interface IPdfService
{
    Task SaveToPdf(ApiResponse weatherDetails);
}