using Domain.ConsoleLogic;
using Domain.Models.WeatherStack;
using Moq;

namespace WeatherConsole.Tests;

public class ResponseProcessorTests
{
    [Fact]
    public async Task
        Given_Null_Response_From_The_Api_When_ProcessApiResponse_Then_Return_False_And_Notify_User()
    {
        // Arrange
        const string mockUserInput1 = "fileName";
        const string mockUserInput2 = "filePath";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, mockUserInput2);
        var mockPdfService = SharedMocks.MockPdfService();
        var mockExitService = SharedMocks.MockExitService();

        var responseProcessor = new ResponseProcessor(mockPdfService.Object, mockExitService.Object, mockConsoleWrapper.Object);

        // Act
        var success = await responseProcessor.ProcessApiResponse("cityName", null);
        
        // Assert
        Assert.False(success);
        
        // Assert that the error message was printed
        mockConsoleWrapper.Verify(x => x.WriteLine("Weather details for \"cityName\" are not available in WeatherStack."), Times.Once);
        // Assert that the weather was not printed
        mockConsoleWrapper.Verify(x => x.WriteLine("\nCurrent weather for :"), Times.Never);
        
        mockExitService.Verify(x => x.VerifyContinue(
            It.IsAny<Func<Task<ApiResponse>>>(), 
            It.IsAny<string>()), 
            Times.Never);
    }
    
    [Fact]
    public async Task
        Given_Response_From_The_Api_When_ProcessApiResponse_Then_Return_True_And_Print_Weather_And_Verify_Continue()
    {
        // Arrange
        const string mockUserInput1 = "fileName";
        const string mockUserInput2 = "filePath";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, mockUserInput2);
        var mockPdfService = SharedMocks.MockPdfService();
        var mockExitService = SharedMocks.MockExitService();

        var responseProcessor = new ResponseProcessor(mockPdfService.Object, mockExitService.Object, mockConsoleWrapper.Object);

        var response = SharedMocks.MockApiResponse();
        
        // Act
        var success = await responseProcessor.ProcessApiResponse("cityName", response);
        
        // Assert
        Assert.True(success);
        
        // Assert that the error message was not printed
        mockConsoleWrapper.Verify(x => x.WriteLine("Weather details for \"cityName\" are not available in WeatherStack."), Times.Never);
        // Assert that the weather was printed
        mockConsoleWrapper.Verify(x => x.WriteLine("\nCurrent weather for :"), Times.Once);
        
        mockExitService.Verify(x => x.VerifyContinue(
            It.IsAny<Func<ApiResponse, Task<ApiResponse>>>(), 
            It.IsAny<ApiResponse>(), 
            It.IsAny<string>()), 
            Times.Once);
    }
}