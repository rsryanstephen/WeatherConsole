using Domain.Models.WeatherStack;

namespace Domain.Interfaces;

public interface IHtmlBuilder
{
    string BuildHtml(ApiResponse weatherDetails);
}