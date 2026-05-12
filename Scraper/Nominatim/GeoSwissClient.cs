using System.Net;
using Commons;

namespace Scraper.Nominatim;

public class GeoSwissClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api3.geo.admin.ch";
    
    // https://api3.geo.admin.ch/rest/services/api/SearchServer?searchText=Binzb%C3%B6sche,%20Obermatthalde,%206045%20Meggen&type=locations

    public GeoSwissClient()
    {
        var handler = new HttpClientHandler
        {
            AutomaticDecompression =
                DecompressionMethods.GZip |
                DecompressionMethods.Deflate |
                DecompressionMethods.Brotli,

            CookieContainer = new CookieContainer()
        };


        _httpClient = new HttpClient(handler);
    }
    
    public async Task<GeoSwissResponse?> GetAddress(string address)
    {

        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/rest/services/api/SearchServer?searchText={address}&type=locations");
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content) || content.Equals("[]", StringComparison.InvariantCulture)) return null;
            var res = content.ToJsonObject<GeoSwissResponse>();
            return res;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Address: {address} | Error: {e.Message}");
            return null;
        }
        
    }
}
public class GeoSwissResponse
{
    public string fuzzy { get; set; }
    public Results[] results { get; set; }
}

public class Results
{
    public Attrs attrs { get; set; }
    public int id { get; set; }
    public int weight { get; set; }
}

public class Attrs
{
    public string detail { get; set; }
    public string featureId { get; set; }
    public string geom_quadindex { get; set; }
    public string geom_st_box2d { get; set; }
    public string label { get; set; }
    public double lat { get; set; }
    public Links[] links { get; set; }
    public double lon { get; set; }
    public int num { get; set; }
    public string objectclass { get; set; }
    public string origin { get; set; }
    public int rank { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public int zoomlevel { get; set; }
}

public class Links
{
    public string href { get; set; }
    public string rel { get; set; }
    public string title { get; set; }
}


