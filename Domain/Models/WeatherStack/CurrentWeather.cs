using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Domain.Models.WeatherStack;

public class CurrentWeather
{
    public CurrentWeather(
        string observationTime,
        decimal temperature,
        int weatherCode,
        List<string> weatherIcons,
        List<string> weatherDescriptions,
        decimal windSpeed,
        decimal windDegree,
        string windDirection,
        decimal pressure,
        decimal precipitation,
        decimal humidity,
        decimal cloudCover,
        int feelsLike,
        decimal uvIndex,
        decimal visibility,
        string isDay
    )
    {
        ObservationTime = observationTime;
        Temperature = temperature;
        WeatherCode = weatherCode;
        WeatherIcons = weatherIcons;
        WeatherDescriptions = weatherDescriptions;
        WindSpeed = windSpeed;
        WindDegree = windDegree;
        WindDirection = windDirection;
        Pressure = pressure;
        Precipitation = precipitation;
        Humidity = humidity;
        CloudCover = cloudCover;
        FeelsLike = feelsLike;
        UvIndex = uvIndex;
        Visibility = visibility;
        IsDay = isDay;
    }

    [JsonProperty("observation_time")]
    [JsonPropertyName("observation_time")]
    public string ObservationTime { get; }

    [JsonProperty("temperature")]
    [JsonPropertyName("temperature")]
    public decimal Temperature { get; }

    [JsonProperty("weather_code")]
    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; }

    [JsonProperty("weather_icons")]
    [JsonPropertyName("weather_icons")]
    public IList<string> WeatherIcons { get; }

    [JsonProperty("weather_descriptions")]
    [JsonPropertyName("weather_descriptions")]
    public IList<string> WeatherDescriptions { get; }

    [JsonProperty("wind_speed")]
    [JsonPropertyName("wind_speed")]
    public decimal WindSpeed { get; }

    [JsonProperty("wind_degree")]
    [JsonPropertyName("wind_degree")]
    public decimal WindDegree { get; }

    [JsonProperty("wind_dir")]
    [JsonPropertyName("wind_dir")]
    public string WindDirection { get; }

    [JsonProperty("pressure")]
    [JsonPropertyName("pressure")]
    public decimal Pressure { get; }

    [JsonProperty("precip")]
    [JsonPropertyName("precip")]
    public decimal Precipitation { get; }

    [JsonProperty("humidity")]
    [JsonPropertyName("humidity")]
    public decimal Humidity { get; }

    [JsonProperty("cloudcover")]
    [JsonPropertyName("cloudcover")]
    public decimal CloudCover { get; }

    [JsonProperty("feelslike")]
    [JsonPropertyName("feelslike")]
    public int FeelsLike { get; }

    [JsonProperty("uv_index")]
    [JsonPropertyName("uv_index")]
    public decimal UvIndex { get; }

    [JsonProperty("visibility")]
    [JsonPropertyName("visibility")]
    public decimal Visibility { get; }

    [JsonProperty("is_day")]
    [JsonPropertyName("is_day")]
    public string IsDay { get; }
}

public class CurrentWeatherRoot
{
    public CurrentWeatherRoot(
        CurrentWeather currentWeather
    )
    {
        CurrentWeather = currentWeather;
    }

    [JsonProperty("current")]
    [JsonPropertyName("current")]
    public CurrentWeather CurrentWeather { get; }
}