using WeatherApiService.Http;
using WeatherApiService.Models;
using IHttpClientFactory = WeatherApiService.Http.IHttpClientFactory;

namespace WeatherApiService.Infrastructure;

public class Startup
{
    private IConfiguration Configuration { get; }
        
    public Startup(IHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("Infrastructure/appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"Infrastructure/appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }
    
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
        
        ConfigureSettings(services);
    }

    private void ConfigureSettings(IServiceCollection services)
    {
        services.Configure<WeatherStackApiSettings>(Configuration.GetSection(nameof(WeatherStackApiSettings)));
    }
}