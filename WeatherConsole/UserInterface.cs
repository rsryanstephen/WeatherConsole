using Domain.Interfaces;

namespace WeatherConsole;

public class UserInterface: IUserInterface
{
    private readonly IResponseProcessor _responseProcessor;
    private readonly IWeatherApiService _weatherApiService;
    private readonly IExitService _exitService;
    private readonly IConsoleWrapper _consoleWrapper;
    
    public UserInterface(
        IWeatherApiService weatherApiService, 
        IResponseProcessor responseProcessor, 
        IExitService exitService,
        IConsoleWrapper consoleWrapper)
    {
        _weatherApiService = weatherApiService;
        _responseProcessor = responseProcessor;
        _exitService = exitService;
        _consoleWrapper = consoleWrapper;
        
        const string welcomeMsg = "\nWelcome to the WeatherStack API client!" +
                                  "\nThis application will allow you to get the current weather details of any city in the world.";
        
        _consoleWrapper.Write(welcomeMsg);
    }
    
    // ReSharper disable once FunctionRecursiveOnAllPaths
    // This is not a problem because an exit option is provided
    public async Task<bool> RequestUserInput()
    {
        var cityName = RequestCityName();

        var response = await _weatherApiService.GetCurrentWeather(cityName);

        if (response == null)
        {
            const string retryMsg = "Would you like to try again? (y/n)";
            return await _exitService.VerifyContinue(RequestUserInput, retryMsg);
        }
            
        await _responseProcessor.ProcessApiResponse(response);

        const string continueMsg = "Would you like to get the current weather details of another city? (y/n)";
        return await _exitService.VerifyContinue(RequestUserInput, continueMsg);
    }
    
    private string RequestCityName()
    {
        _consoleWrapper.WriteLine("");
        _consoleWrapper.WriteLine("Enter city name to get current weather:");

        // Create a string variable and get user input from the keyboard and store it in the variable
        var cityName = _consoleWrapper.ReadLine();
        
        if (string.IsNullOrWhiteSpace(cityName))
        {
            _consoleWrapper.WriteLine("City name cannot be empty");
            return RequestCityName();
        }

        return cityName;
    }

}