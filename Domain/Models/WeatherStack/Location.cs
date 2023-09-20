using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Domain.Models.WeatherStack;
public class Location
{
    public Location(
        string name,
        string country,
        string region,
        string lat,
        string lon,
        string timezoneId,
        string localtime,
        int localtimeEpoch,
        string utcOffset
    )
    {
        Name = name;
        Country = country;
        Region = region;
        Lat = lat;
        Lon = lon;
        TimezoneId = timezoneId;
        Localtime = localtime;
        LocaltimeEpoch = localtimeEpoch;
        UtcOffset = utcOffset;
    }

    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonProperty("country")]
    [JsonPropertyName("country")]
    public string Country { get; }

    [JsonProperty("region")]
    [JsonPropertyName("region")]
    public string Region { get; }

    [JsonProperty("lat")]
    [JsonPropertyName("lat")]
    public string Lat { get; }

    [JsonProperty("lon")]
    [JsonPropertyName("lon")]
    public string Lon { get; }

    [JsonProperty("timezone_id")]
    [JsonPropertyName("timezone_id")]
    public string TimezoneId { get; }

    [JsonProperty("localtime")]
    [JsonPropertyName("localtime")]
    public string Localtime { get; }

    [JsonProperty("localtime_epoch")]
    [JsonPropertyName("localtime_epoch")]
    public int LocaltimeEpoch { get; }

    [JsonProperty("utc_offset")]
    [JsonPropertyName("utc_offset")]
    public string UtcOffset { get; }
}

public class LocationRoot
{
    public LocationRoot(
        Location location
    )
    {
        Location = location;
    }

    [JsonProperty("location")]
    [JsonPropertyName("location")]
    public Location Location { get; }
}