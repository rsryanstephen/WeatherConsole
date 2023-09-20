using Domain.Interfaces;
using Domain.Models.WeatherStack;
using Moq;

namespace WeatherConsole.Tests;

public static class SharedMocks
{
    public static Mock<IConsoleWrapper> MockConsoleWrapper(string userInput1, string userInput2)
    {
        var mock = new Mock<IConsoleWrapper>();
        mock.SetupSequence(x => x.ReadLine()).Returns(userInput1).Returns(userInput2);
        mock.Setup(x => x.ReadKey()).Returns(new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false));
        return mock;
    }
    
    public static Mock<IExitService> MockExitService(bool success = true)
    {
        var mock = new Mock<IExitService>();
        mock.Setup(x => x.VerifyContinue(It.IsAny<Func<Task<bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(success);
        
        mock.Setup(x => x.VerifyContinue(It.IsAny<Func<Task<string>>>(), It.IsAny<string>()))
            .ReturnsAsync("fileName.pdf");

        mock.Setup(x => x.VerifyContinue(It.IsAny<Func<string, Task<string>>>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("filePath");

        var mockApiResponse = MockApiResponse();
        mock.Setup(x => x.VerifyContinue(It.IsAny<Func<Task<ApiResponse>>>(), It.IsAny<string>()))
            .ReturnsAsync(mockApiResponse);

        mock.Setup(x => x.VerifyContinue(It.IsAny<Func<ApiResponse, Task<ApiResponse>>>(), It.IsAny<ApiResponse>(), It.IsAny<string>()))
            .ReturnsAsync(mockApiResponse);

        return mock;
    }
    
    public static ApiResponse MockApiResponse()
    {
        var request = new Request("", "", "", "");
        var location = new Location("", "", "", "", "", "", "", 0, "");
        var currentWeather = new CurrentWeather("", 0, 0, new List<string>(), new List<string>(), 0, 0, "", 0, 0, 0, 0,
            0, 0, 0, "");
        
        var mockApiResponse = new ApiResponse(request, location, currentWeather);
        return mockApiResponse;
    }

    public static Mock<IPdfService> MockPdfService()
    {
        var mock = new Mock<IPdfService>();
        mock.Setup(x => x.SaveToPdf(It.IsAny<ApiResponse>()));

        return mock;
    }

    public static Mock<IResponseProcessor> MockResponseProcessor()
    {
        var mock = new Mock<IResponseProcessor>();
        mock.Setup(x => x.ProcessApiResponse(It.IsAny<ApiResponse>()));

        return mock;
    }

    public static Mock<IWeatherApiService> MockWeatherApiService(string cityName, ApiResponse? returnVal)
    {
        var mock = new Mock<IWeatherApiService>();
        
        mock.Setup(x => x.GetCurrentWeather(cityName))
            .ReturnsAsync(returnVal);

        return mock;
    }
    
    
}