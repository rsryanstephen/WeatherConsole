using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Domain.Models.WeatherStack;

public class ApiResponse
{
    public ApiResponse(
        Request request,
        Location location,
        CurrentWeather currentWeather
    )
    {
        Request = request;
        Location = location;
        CurrentWeather = currentWeather;
    }
    
    [JsonProperty("request")]
    [JsonPropertyName("request")]
    public Request Request { get; }
    
    [JsonProperty("location")]
    [JsonPropertyName("location")]
    public Location Location { get; }
    
    [JsonProperty("current")]
    [JsonPropertyName("current")]
    public CurrentWeather CurrentWeather { get; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
    
        var other = (ApiResponse) obj;
        return Request.Equals(other.Request) 
               && Location.Equals(other.Location) 
               && CurrentWeather.Equals(other.CurrentWeather);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Request, Location, CurrentWeather);
    }
    
    public static bool operator ==(ApiResponse? left, ApiResponse? right)
    {
        return Equals(left, right);
    }
    
    public static bool operator !=(ApiResponse? left, ApiResponse? right)
    {
        return !Equals(left, right);
    }
}