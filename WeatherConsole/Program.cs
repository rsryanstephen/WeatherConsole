using Domain.ConsoleLogic;
using Domain.Interfaces;
using Domain.Utilities;
using Domain.Utilities.PdfOutput;
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
        var outputProvider = new ResponseProcessor(pdfService, exitService, consoleWrapper);
        
        var weatherApiService = ApiServiceFactory.GetWeatherApiService();
        
        return new UserInterface(weatherApiService, outputProvider, exitService, consoleWrapper);
    }
}