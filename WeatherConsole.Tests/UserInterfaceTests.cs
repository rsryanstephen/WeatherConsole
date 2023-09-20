using Domain.Core;
using Moq;

namespace WeatherConsole.Tests;

public class UserInterfaceTests
{
    [Fact]
    public async Task
        Given_Valid_User_Input_For_City_Name_And_Valid_Response_From_The_Api_When_RequestUserInput_Then_Return_True_And_Follow_THe_Expected_Happy_Path()
    {
        // Arrange
        const string mockUserInput1 = "cityName";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, "");
        var mockResponseProcessor = SharedMocks.MockResponseProcessor(mockUserInput1, true);
        var mockWeatherDetails = SharedMocks.MockApiResponse();
        var mockWeatherApiService = SharedMocks.MockWeatherApiService(mockUserInput1, mockWeatherDetails);
        var mockExitService = SharedMocks.MockExitService();

        var userInterface = new UserInterface(
            mockWeatherApiService.Object, 
            mockResponseProcessor.Object, 
            mockExitService.Object, 
            mockConsoleWrapper.Object);

        // Act
        var success = await userInterface.RequestUserInput();
        
        // Assert
        Assert.True(success);
        
        // Assert that mockWeatherApiService was called with the first user input
        mockWeatherApiService.Verify(x => x.GetCurrentWeather(mockUserInput1), Times.Once);
        
        // Assert that mockResponseProcessor was called with the first user input and the response from the api
        mockResponseProcessor.Verify(x => x.ProcessApiResponse(mockUserInput1, mockWeatherDetails), Times.Once);
        
        // Assert that the starting message was printed once
        mockConsoleWrapper.Verify(x => x.WriteLine("Enter city name to get current weather:"), Times.Once);
        
        // Assert that the happy path message was printed
        const string expectedMsg = "Would you like to get the current weather details of another city? (y/n)";
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<Task<bool>>>(), expectedMsg), Times.Once);
        
        // Assert that the error message was not printed
        mockConsoleWrapper.Verify(x => x.WriteLine("City name cannot be empty"), Times.Never);
        mockConsoleWrapper.Verify(x => x.WriteLine("Would you like to try again? (y/n)"), Times.Never);
        
        mockExitService.Verify(x => x.VerifyContinue(
                It.IsAny<Func<Task<bool>>>(), 
                It.IsAny<string>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task
        Given_Empty_User_Input_For_City_Name_And_Valid_Response_From_The_Api_When_RequestUserInput_Then_Return_True_And_Follow_The_Expected_Happy_Path_After_Retry_Of_User_Input()
    {
        // Arrange
        const string mockUserInput1 = "  ";
        const string mockUserInput2 = "  city";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, mockUserInput2);
        var mockResponseProcessor = SharedMocks.MockResponseProcessor(mockUserInput2, true);
        var mockWeatherDetails = SharedMocks.MockApiResponse();
        var mockWeatherApiService = SharedMocks.MockWeatherApiService(mockUserInput2, mockWeatherDetails);
        var mockExitService = SharedMocks.MockExitService();

        var userInterface = new UserInterface(
            mockWeatherApiService.Object, 
            mockResponseProcessor.Object, 
            mockExitService.Object, 
            mockConsoleWrapper.Object);

        // Act
        var success = await userInterface.RequestUserInput();
        
        // Assert that it eventually succeeded after the retry
        Assert.True(success);
        
        // Assert that the starting message was printed twice for the restart
        mockConsoleWrapper.Verify(x => x.WriteLine("Enter city name to get current weather:"), Times.Exactly(2));
        
        // Assert that the error message was printed once
        mockConsoleWrapper.Verify(x => x.WriteLine("City name cannot be empty"), Times.Once);
        
        // Assert that the retry message for the full method was not printed
        const string expectedRetryMsg = "Would you like to try again? (y/n)";
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<Task<bool>>>(), expectedRetryMsg), Times.Never);
        
        // Assert that the happy path message was eventually printed
        const string expectedMsg = "Would you like to get the current weather details of another city? (y/n)";
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<Task<bool>>>(), expectedMsg), Times.Once);
        
        // Assert that mockWeatherApiService was called with the second user input
        mockWeatherApiService.Verify(x => x.GetCurrentWeather(mockUserInput2), Times.Once);
        
        // Assert that mockResponseProcessor was called with the second user input and the response from the api
        mockResponseProcessor.Verify(x => x.ProcessApiResponse(mockUserInput2, mockWeatherDetails), Times.Once);
        
        
        mockExitService.Verify(x => x.VerifyContinue(
                It.IsAny<Func<Task<bool>>>(), 
                It.IsAny<string>()), 
            Times.Once);
    }
    
    [Fact]
    public async Task
        Given_Valid_User_Input_But_Invalid_Response_From_The_Api_When_RequestUserInput_Then_Return_False_And_Request_Retry()
    {
        // Arrange
        const string mockUserInput1 = "city";
        const string mockUserInput2 = "city";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, mockUserInput2);
        var mockResponseProcessor = SharedMocks.MockResponseProcessor(mockUserInput2, false);
        var mockWeatherDetails = SharedMocks.MockApiResponse();
        var mockWeatherApiService = SharedMocks.MockWeatherApiService(mockUserInput2, mockWeatherDetails);
        var mockExitService = SharedMocks.MockExitService(false);

        var userInterface = new UserInterface(
            mockWeatherApiService.Object, 
            mockResponseProcessor.Object, 
            mockExitService.Object, 
            mockConsoleWrapper.Object);

        // Act
        var success = await userInterface.RequestUserInput();
        
        // Assert that it eventually succeeded after the retry
        Assert.False(success);

        // Assert that the error message for user input was not printed
        mockConsoleWrapper.Verify(x => x.WriteLine("City name cannot be empty"), Times.Never);
        
        // Assert that the retry message was printed
        const string expectedRetryMsg = "Would you like to try again? (y/n)";
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<Task<bool>>>(), expectedRetryMsg), Times.Once);
        
        // Assert that the happy path message was not printed
        const string expectedMsg = "Would you like to get the current weather details of another city? (y/n)";
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<Task<bool>>>(), expectedMsg), Times.Never);
        
        // Assert that mockWeatherApiService was called with the first user input
        mockWeatherApiService.Verify(x => x.GetCurrentWeather(mockUserInput1), Times.Once);
        
        // Assert that mockResponseProcessor was called with the first user input and the response from the api
        mockResponseProcessor.Verify(x => x.ProcessApiResponse(mockUserInput1, mockWeatherDetails), Times.Once);
        
        
        mockExitService.Verify(x => x.VerifyContinue(
                It.IsAny<Func<Task<bool>>>(), 
                It.IsAny<string>()), 
            Times.Once);
    }
}