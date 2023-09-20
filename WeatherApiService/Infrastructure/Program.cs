using Autofac.Extensions.DependencyInjection;

namespace WeatherApiService.Infrastructure;

public class Program
{
    public static void Main(string[] args)
    {
        var title = typeof(Program).Namespace;
        if (title != null) Console.Title = title;
        
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureServices(services => services.AddAutofac())
                    .UseStartup<Startup>();
            });
}