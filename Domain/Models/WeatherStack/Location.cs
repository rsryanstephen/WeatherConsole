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
    public string Name { get; }

    [JsonProperty("country")]
    public string Country { get; }

    [JsonProperty("region")]
    public string Region { get; }

    [JsonProperty("lat")]
    public string Lat { get; }

    [JsonProperty("lon")]
    public string Lon { get; }

    [JsonProperty("timezone_id")]
    public string TimezoneId { get; }

    [JsonProperty("localtime")]
    public string Localtime { get; }

    [JsonProperty("localtime_epoch")]
    public int LocaltimeEpoch { get; }

    [JsonProperty("utc_offset")]
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
    public Location Location { get; }
}