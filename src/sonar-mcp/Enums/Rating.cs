using System.Text.Json.Serialization;

namespace Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Rating : byte
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5
    }
}