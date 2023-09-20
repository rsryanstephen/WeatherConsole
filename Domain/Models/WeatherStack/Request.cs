using System.Text.Json.Serialization;
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
    [JsonPropertyName("type")]
    public string Type { get; }

    [JsonProperty("query")]
    [JsonPropertyName("query")]
    public string Query { get; }

    [JsonProperty("language")]
    [JsonPropertyName("language")]
    public string Language { get; }

    [JsonProperty("unit")]
    [JsonPropertyName("unit")]
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
    [JsonPropertyName("request")]
    public Request Request { get; }
}

