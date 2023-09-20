using Newtonsoft.Json;

namespace Domain.Models.WeatherStack;

public class Request
{
    public Request(
        string type,
        string query,
        string language,
        string unit
    )
    {
        Type = type;
        Query = query;
        Language = language;
        Unit = unit;
    }

    [JsonProperty("type")]
    public string Type { get; }

    [JsonProperty("query")]
    public string Query { get; }

    [JsonProperty("language")]
    public string Language { get; }

    [JsonProperty("unit")]
    public string Unit { get; }
}

public class RequestRoot
{
    public RequestRoot(
        Request request
    )
    {
        Request = request;
    }

    [JsonProperty("request")]
    public Request Request { get; }
}

