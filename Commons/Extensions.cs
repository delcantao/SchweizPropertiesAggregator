using System.Text.Json;

namespace Commons;

public static class Extensions
{
    public static T? ToJsonObject<T>(this string obj, JsonSerializerOptions options = null)
    {
        return JsonSerializer.Deserialize<T>(obj, options);
    }
    
       public static string ToJson(this object obj, JsonSerializerOptions options = null)
    {
        return JsonSerializer.Serialize(obj, options);
    }
}