using WeatherApiService.Models;

namespace WeatherApiService.Infrastructure;

public static class Settings
{
    public static WeatherStackApiSettings GetWeatherStackSettings() =>
         GetConfig().GetSection(nameof(WeatherStackApiSettings))
             .Get<WeatherStackApiSettings>();

    private static IConfiguration GetConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Infrastructure/appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}