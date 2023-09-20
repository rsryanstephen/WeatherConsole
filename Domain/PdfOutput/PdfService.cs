using Domain.Interfaces;
using Domain.Models.WeatherStack;

namespace Domain.PdfOutput;

public class PdfService: IPdfService
{
    private readonly IFilePathService _filePathService;
    private readonly IHtmlBuilder _htmlBuilder;
    private readonly IConsoleWrapper _consoleWrapper;

    public PdfService(IFilePathService filePathService, IHtmlBuilder htmlBuilder, IConsoleWrapper consoleWrapper)
    {
        _filePathService = filePathService;
        _htmlBuilder = htmlBuilder;
        _consoleWrapper = consoleWrapper;
    }
    
    public async Task SaveToPdf(ApiResponse weatherDetails)
    {
        var filePath = await _filePathService.Get();
        var html = _htmlBuilder.BuildHtml(weatherDetails);
        
        await SaveHtmlToPdf(html, filePath);
    }
    
    private async Task SaveHtmlToPdf(string html, string filepath)
    {
        var renderer = new ChromePdfRenderer
        {
            RenderingOptions =
            {
                HtmlFooter = new HtmlHeaderFooter
                {
                    MaxHeight = 15, //millimeters
                    HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
                    DrawDividerLine = true
                }
            }
        };

        using var pdf = await renderer.RenderHtmlAsPdfAsync(html);

        pdf.SaveAs(filepath);
        
        _consoleWrapper.WriteLine($"\nWeather saved to \"{filepath}\"");
    }
    
}