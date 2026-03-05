using Newtonsoft.Json;

namespace MSWMS.Infrastructure.Helpers;

public static class JsonParser
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        MissingMemberHandling = MissingMemberHandling.Ignore,

        NullValueHandling = NullValueHandling.Include,
        
    };

    public static T Parse<T>(string json)
        => JsonConvert.DeserializeObject<T>(json, Settings)
           ?? throw new JsonSerializationException("Empty/Invalid JSON (Deserialize return null).");
}