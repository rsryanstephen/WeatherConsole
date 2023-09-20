using Domain.Core;
using Domain.Interfaces;
using WeatherApiService.ApiClients;
using WeatherApiService.Http;

namespace WeatherApiService.Factories;

public static class ApiServiceFactory
{
    public static IWeatherApiService GetWeatherApiService()
    {
        // other clients?
        var weatherService = GetWeatherStackClient();
        
        return weatherService;
    }

    private static IWeatherApiService GetWeatherStackClient()
    {
        var consoleWrapper = new ConsoleWrapper();
        var factory = new HttpClientFactory();
        var httpClient = new HttpClientWrapper(factory);
        var weatherService = new WeatherStackClient(httpClient, consoleWrapper);

        return weatherService;
    }
}