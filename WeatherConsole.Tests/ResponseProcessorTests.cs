using Domain.Core;
using Domain.Models.WeatherStack;
using Moq;

namespace WeatherConsole.Tests;

public class ResponseProcessorTests
{
    [Fact]
    public async Task
        Given_Response_From_The_Api_When_ProcessApiResponse_Then_Offer_To_Save_To_Pdf()
    {
        // Arrange
        const string mockUserInput1 = "fileName";
        const string mockUserInput2 = "filePath";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, mockUserInput2);
        var mockPdfService = SharedMocks.MockPdfService();
        var mockExitService = SharedMocks.MockExitService();

        var responseProcessor = new ApiResponseProcessor(mockPdfService.Object, mockExitService.Object, mockConsoleWrapper.Object);

        var mockApiResponse = SharedMocks.MockApiResponse();
        // Act
        await responseProcessor.ProcessApiResponse(mockApiResponse);
        
        // Assert
        mockExitService.Verify(x => x.VerifyContinue(
            It.IsAny<Func<ApiResponse, Task<ApiResponse>>>(),
            mockApiResponse,
            "\nSave current weather to a pdf document? (y/n)"), 
            Times.Once);
    }
}