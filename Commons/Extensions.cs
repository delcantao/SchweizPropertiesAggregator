using System.Text.Json;
using System.Text.RegularExpressions;

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
    public static string OnlyNumbersReturnNumber(this string text)
    {
        var number = text == null ? "0" : Regex.Replace(text, "[^0-9]", "");
        return string.IsNullOrEmpty(number) ? "0" : number;
    }

}