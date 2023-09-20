using Domain.Interfaces;
using Domain.Utilities;
using Moq;

namespace WeatherConsole.Tests;

public class FilePathServiceTests
{
    [Fact]
    public async Task Given_FileName_And_FilePath_When_Get_FilePath_Then_Run_To_Existing_Directory_Check_With_Expected_Console_Output()
    {
        // Arrange
        const string mockUserInput1 = "fileName";
        const string mockUserInput2 = "filePath";
        var mockConsoleWrapper = SharedMocks.MockConsoleWrapper(mockUserInput1, mockUserInput2);
        var mockExitService = SharedMocks.MockExitService();
        
        var filePathService = new FilePathService(mockExitService.Object, mockConsoleWrapper.Object);
        
        // Act
        await filePathService.Get();
        
        // Assert
        // Assert that file name was requested:
        mockConsoleWrapper.Verify(x => x.WriteLine("\nName of file:"), Times.Once);
        
        // Assert that file path was requested:
        mockConsoleWrapper.Verify(x => x.WriteLine(
            "\nPlease enter the location (filepath) that you would like to save \"fileName.pdf\" to."), Times.Once);
        
        // Assert that filepath was checked for existence:
        mockConsoleWrapper.Verify(x => x.WriteLine(
            "Provided filePath does not exist: \"filePath\""), Times.Once);
        
        // Assert that the user was asked to try again:
        mockExitService.Verify(x => x.VerifyContinue(
            It.IsAny<Func<string, Task<string>>>(), 
            $"{mockUserInput1}.pdf", 
            It.IsAny<string>()), 
            Times.Once);
    }

    [Fact]
    public async Task Given_Empty_FileName_Twice_And_Empty_FilePath_When_Get_FilePath_Then_VerifyExit_And_Fetch_Current_Directory_With_Expected_Console_Output()
    {
        // Arrange
        var mockUserInput1 = string.Empty;
        const string mockUserInput2 = " ";
        const string mockUserInput3 = " fileName";
        const string mockUserInput4 = "";
        const string mockUserInput5 = "filePath";
        
        var mockConsoleWrapper = new Mock<IConsoleWrapper>();
        mockConsoleWrapper.SetupSequence(x => x.ReadLine())
            .Returns(mockUserInput1)
            .Returns(mockUserInput2)
            .Returns(mockUserInput3)
            .Returns(mockUserInput4)
            .Returns(mockUserInput5);
        
        var mockExitService = SharedMocks.MockExitService();
        var filePathService = new FilePathService(mockExitService.Object, mockConsoleWrapper.Object);
        
        // Act
        var result = await filePathService.Get();
        
        // Assert
        // Assert that file name was requested and validated:
        mockConsoleWrapper.Verify(x => x.WriteLine("\nProvided fileName is empty!"), Times.Once);
        
        // Calls VerifyContinue on GetVerifiedFileName for empty file names passed in
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<Task<string>>>(), It.IsAny<string>()), Times.AtLeastOnce);
        // Should not call VerifyContinue on GetVerifiedFilePath because an empty file path defaults to the current directory
        mockExitService.Verify(x => x.VerifyContinue(It.IsAny<Func<string, Task<string>>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        
        // Assert that file path was requested:
        mockConsoleWrapper.Verify(x => x.WriteLine(
            "\nPlease enter the location (filepath) that you would like to save \"fileName.pdf\" to."), Times.Once);
        
        // Assert that filepath exists:
        mockConsoleWrapper.Verify(x => x.WriteLine(
            "Provided filePath does not exist: \"filePath\""), Times.Never);
        
        // Assert that the file path is the current directory
        var expectedFilePath = Directory.GetCurrentDirectory();
        // Assert that the file name is trimmed and the pdf suffix is attached
        const string expectedFileName = "fileName.pdf";
        
        Assert.Equal($"{expectedFilePath}\\{expectedFileName}", result);
    }
}