using Common.Logging;
using Domain.Interfaces;
using Domain.Models.WeatherStack;
using Microsoft.Extensions.Options;
using WeatherApiService.Http;
using WeatherApiService.Infrastructure;
using WeatherApiService.Models;

namespace WeatherApiService.ApiClients;

public class WeatherStackClient : IWeatherApiService
{
    private static readonly ILog _log = LogManager.GetLogger<WeatherStackClient>();
    
    private readonly IHttpClientWrapper _httpClient;
    private readonly WeatherStackApiSettings _weatherStackApiSettings;
    private readonly IConsoleWrapper _consoleWrapper;

    public WeatherStackClient(
        IHttpClientWrapper client,
        IConsoleWrapper consoleWrapper,
        IOptions<WeatherStackApiSettings>? weatherStackApiOptions = null)
    {
        _httpClient = client;
        _consoleWrapper = consoleWrapper;
        _weatherStackApiSettings = weatherStackApiOptions?.Value ?? Settings.GetWeatherStackSettings();
    }
    
    public async Task<ApiResponse?> GetCurrentWeather(string cityName)
    {
        return await GetWeather(cityName, "current");
    }

    private async Task<ApiResponse?> GetWeather(string cityName, string resource)
    {
        if (_weatherStackApiSettings.ApiKey == null || _weatherStackApiSettings.ApiUrl == null)
        {
            throw new Exception("WeatherStackApiSettings not configured correctly");
        }

        try
        {
            return await RequestWithHttpClient(cityName, resource);
        }
        catch (KeyNotFoundException)
        {
            _consoleWrapper.WriteLine($"Weather details for \"{cityName}\" are not available in WeatherStack.");
            _consoleWrapper.WriteLine("Are you sure that you spelt the name of that city correctly?");

            // Handled by the calling method
            return null;
        }
        catch (Exception e)
        {
            var msg = $"Error while trying to get weather for \"{cityName}\" from {_weatherStackApiSettings.ApiUrl}:" +
                      $"{Environment.NewLine}{e.Message}";

            _log.Error(msg, e);

            throw new Exception(msg, e);
        }
    }

    private async Task<ApiResponse> RequestWithHttpClient(string cityName, string resource)
    {
        var requestUri = GetHttpRequestUrl(cityName, resource);
        var response = await _httpClient.GetFromHttpResponseAsync<ApiResponse>(requestUri);
    
        return response;
    }

    private string GetHttpRequestUrl(string cityName, string resource)
    {
        return _weatherStackApiSettings.ApiUrl
               + resource
               + "?access_key=" + _weatherStackApiSettings.ApiKey
               + "&query=" + cityName;
    }
}