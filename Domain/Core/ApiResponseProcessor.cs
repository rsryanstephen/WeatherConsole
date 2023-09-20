using Domain.Interfaces;
using Domain.Models.WeatherStack;

namespace Domain.Core;

public class ApiResponseProcessor: IResponseProcessor
{
    private readonly IPdfService _pdfService;
    private readonly IExitService _exitService;
    private readonly IConsoleWrapper _consoleWrapper;

    public ApiResponseProcessor(IPdfService pdfService, IExitService exitService, IConsoleWrapper consoleWrapper)
    {
        _pdfService = pdfService;
        _exitService = exitService;
        _consoleWrapper = consoleWrapper;
    }

    /// <returns>boolean success variable</returns>
    public async Task<bool> ProcessApiResponse(string cityName, ApiResponse? response)
    {
        if (response == null || response?.CurrentWeather?.ObservationTime == null) 
        {
            _consoleWrapper.WriteLine($"Weather details for \"{cityName}\" are not available in WeatherStack.");
            _consoleWrapper.WriteLine("Are you sure that you spelt the name of that city correctly?");
            return await Task.FromResult(false);
        }

        PrintWeatherSummary(response);
        
        const string continueMsg = "\nSave current weather to a pdf document? (y/n)";
        
        await _exitService.VerifyContinue(SaveWeatherToPdf, response, continueMsg);

        return true;
    }
    
    private async Task<ApiResponse> SaveWeatherToPdf(ApiResponse response)
    {
        try
        {
            await _pdfService.SaveToPdf(response);
        }
        catch (Exception e)
        {
            _consoleWrapper.WriteLine("\nAn error occurred while trying to save the weather to a pdf.");
            _consoleWrapper.WriteLine(e.Message);
            
            const string continueMsg = "Due to the above error, we failed to save it to a pdf." +
                                       "\nWould you like to try again? (y/n)";
            
            await _exitService.VerifyContinue(SaveWeatherToPdf, response, continueMsg);
        }

        return response;
    }

    private void PrintWeatherSummary(ApiResponse response)
    {
        _consoleWrapper.WriteLine($"\nCurrent weather for {response.Location.Name}:");
        
        _consoleWrapper.WriteLine($"City: {response.Location.Name}");
        _consoleWrapper.WriteLine($"Temperature: {response.CurrentWeather.Temperature}");
        _consoleWrapper.WriteLine($"Feels like: {response.CurrentWeather.FeelsLike}");
        _consoleWrapper.WriteLine($"Wind speed: {response.CurrentWeather.WindSpeed}");
        _consoleWrapper.WriteLine($"Wind direction: {response.CurrentWeather.WindDirection}");
        _consoleWrapper.WriteLine($"Humidity: {response.CurrentWeather.Humidity}");
        
        PrintDescriptions(response.CurrentWeather.WeatherDescriptions);
    }
    
    private void PrintDescriptions(ICollection<string> descriptions)
    {
        _consoleWrapper.WriteLine($"Weather description: {descriptions.FirstOrDefault()}");
        
        if (descriptions.Count <= 1) return;
        
        foreach (var description in descriptions.Skip(1))
        {
            _consoleWrapper.WriteLine($"{description}");
        }
    }
}