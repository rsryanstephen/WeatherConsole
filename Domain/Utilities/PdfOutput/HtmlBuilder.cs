using Domain.Interfaces;
using Domain.Models.WeatherStack;

namespace Domain.Utilities.PdfOutput;

public class HtmlBuilder : IHtmlBuilder
{
    public string BuildHtml(ApiResponse weatherDetails)
    {
        var current = weatherDetails.CurrentWeather;
        var descriptions = current.WeatherDescriptions;
        var icons = current.WeatherIcons;

        var html = @$"
            {GetHeadings(weatherDetails)}

            <h2>Current Weather</h2>
            <h3>{GetDescriptions(descriptions)}</h3>
            {GetIcons(icons)}

            {GetCurrentWeatherTable(current)}
            {GetObservationTime(current)}";

        return html;
    }

    private static string GetHeadings(ApiResponse weatherDetails)
    {
        var location = weatherDetails.Location;

        var cityName = location.Name;
        var region = location.Region;
        var country = location.Country;
        var localTime = location.Localtime;

        var regionAndCountry = string.IsNullOrWhiteSpace(region) ? country : $"{region}, {country}";

        var heading = $"Weather Details for {cityName}";
        var locationSubHeader = $"{cityName}, {regionAndCountry} at {localTime}";

        return @$"<h1>{heading}</h1>
            <p style=""font-style: italic; font-size: 20px;""> {locationSubHeader} </p>";
    }

    private static string GetDescriptions(ICollection<string> descriptions)
    {
        if (descriptions.Count == 0) return string.Empty;

        var description = descriptions.FirstOrDefault() ?? string.Empty;

        return descriptions.Count == 1
            ? $"{description}"
            : $"{description}, {GetDescriptions(descriptions.Skip(1).ToList())}";
    }

    private static string GetIcons(ICollection<string> icons)
    {
        if (icons.Count == 0) return string.Empty;

        var icon = icons.FirstOrDefault() ?? string.Empty;

        return icons.Count == 1
            ? $"<link rel=\"icon\" type=\"image/png\" href=\"{icon}\" />"
            : $"<link rel=\"icon\" type=\"image/png\" href=\"{icon}\" />\n{GetIcons(icons.Skip(1).ToList())}";
    }

    private static string GetObservationTime(CurrentWeather current) => 
        $@"<p style=""font-style: italic;"">Observed at: {current.ObservationTime} UTC</p>";

    private static string TableSelector() => @"<table style=""text-align: left;"">";

    private static string TableHeader(string header) => @$"<th style=""padding: 10px 20px 0px 0px;"">{header}</th>";

    private static string GetCurrentWeatherTable(CurrentWeather current)
    {
        return @$"{TableSelector()}
          <tr>
            {TableHeader("Temperature")}
            {TableHeader("Feels Like")}
            {TableHeader("Humidity")}
            {TableHeader("Cloud Cover")}
          </tr>
          <tr>
            <td>{current.Temperature}°C</td>
            <td>{current.FeelsLike}°C</td>
            <td>{current.Humidity}%</td>
            <td>{current.CloudCover}%</td>
          </tr>
          <tr>
            {TableHeader("Wind Speed")}
            {TableHeader("Wind Direction")}
            {TableHeader("UV Index")}
            {TableHeader("Visibility")}
          </tr>
          <tr>
            <td>{current.WindSpeed} km/h</td>
            <td>{current.WindDirection}</td>
            <td>{current.UvIndex}</td>
            <td>{current.Visibility} km</td>
          </tr>
          <tr>
            {TableHeader("Pressure")}
            {TableHeader("Precipitation")}
            {TableHeader("Is Day")}
            {TableHeader("Wind Degree")}
          </tr>
          <tr>
            <td>{current.Pressure} mb</td>
            <td>{current.Precipitation} mm</td>
            <td>{current.IsDay}</td>
            <td>{current.WindDegree}</td>
          </tr>
        </table>";
    }
}