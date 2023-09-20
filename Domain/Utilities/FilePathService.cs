using Domain.Interfaces;

namespace Domain.Utilities;

public class FilePathService : IFilePathService
{
    private readonly IExitService _exitService;
    private readonly IConsoleWrapper _consoleWrapper;

    public FilePathService(IExitService exitService, IConsoleWrapper consoleWrapper)
    {
        _exitService = exitService;
        _consoleWrapper = consoleWrapper;
    }
    
    public async Task<string> Get()
    {
        var fileName = await GetVerifiedFileName();
        var filePath = await GetVerifiedFilePath(fileName);

        return $"{filePath}\\{fileName}";
    }

    private async Task<string> GetVerifiedFileName()
    {
        _consoleWrapper.WriteLine("\nName of file:");
        var fileName = _consoleWrapper.ReadLine();
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            const string msg = "\nProvided fileName is empty!";
    
            _consoleWrapper.WriteLine(msg);
            return await _exitService.VerifyContinue(GetVerifiedFileName);
        }
        
        return AttachPdfSuffix(fileName).Trim();
    }
    
    private static string AttachPdfSuffix(string filePath) =>
        filePath.EndsWith(".pdf") ? filePath : filePath + ".pdf";

    private async Task<string> GetVerifiedFilePath(string fileName)
    {
        _consoleWrapper.WriteLine($"\nPlease enter the location (filepath) that you would like to save \"{fileName}\" to.");
        _consoleWrapper.WriteLine("(Leave blank to default to the current directory)");
        
        var filePath = _consoleWrapper.ReadLine();
        
        if (string.IsNullOrWhiteSpace(filePath))
        {
            filePath = Directory.GetCurrentDirectory();
        }

        if (!ValidateFilePath(filePath))
        {
            return await _exitService.VerifyContinue(GetVerifiedFilePath, fileName);
        }

        return filePath;
    }

    private bool ValidateFilePath(string? filePath)
    {
        if (!Directory.Exists(filePath))
        {
            var msg = $"Provided filePath does not exist: \"{filePath}\"";

            _consoleWrapper.WriteLine(msg);
            return false;
        }

        return true;
    }
}