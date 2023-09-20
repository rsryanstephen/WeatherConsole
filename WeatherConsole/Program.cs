using Domain.Core;
using Domain.Interfaces;
using Domain.PdfOutput;
using WeatherApiService.Factories;

namespace WeatherConsole;

internal abstract class UiConsole
{
    public static async Task Main()
    {
        await GetUserInterface().RequestUserInput();
    }

    private static IUserInterface GetUserInterface()
    {
        var consoleWrapper = new ConsoleWrapper();
        var exitService = new ExitService(consoleWrapper);
        var filePathValidator = new FilePathService(exitService, consoleWrapper);
        var htmlGenerator = new HtmlBuilder();
        var pdfService = new PdfService(filePathValidator, htmlGenerator, consoleWrapper);
        var outputProvider = new ApiResponseProcessor(pdfService, exitService, consoleWrapper);
        
        var weatherApiService = ApiServiceFactory.GetWeatherApiService();
        
        return new UserInterface(weatherApiService, outputProvider, exitService, consoleWrapper);
    }
}