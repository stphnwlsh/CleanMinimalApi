namespace CleanMinimalApi.Presentation.Tests.Integration.Extensions;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonSerializerExtensions
{
    private static readonly JsonSerializerOptions Options = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    public static string Serialize(this object input)
    {
        return JsonSerializer.Serialize(input, Options);
    }

    public static T Deserialize<T>(this string input)
    {
        return JsonSerializer.Deserialize<T>(input, Options);
    }
}
