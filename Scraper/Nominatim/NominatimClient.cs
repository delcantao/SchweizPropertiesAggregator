using System.Net;
using Commons;

namespace Scraper.Nominatim;

public class NominatimClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://192.168.1.68:8080"; 


    public NominatimClient()
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
    
    public async Task<NominatimResponse?> GetAddress(string address)
    {

        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/search?q={address}&format=jsonv2&addressdetails=1");
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content) || content.Equals("[]", StringComparison.InvariantCulture)) return null;
            var res = content.ToJsonObject<NominatimResponse[]>();
            return res[0];
        }
        catch (Exception e)
        {
            Console.WriteLine($"Address: {address} | Error: {e.Message}");
            return null;
        }
        
    }
}

public class NominatimResponse
{
    public int place_id { get; set; }
    public string licence { get; set; }
    public string osm_type { get; set; }
    public long osm_id { get; set; }
    public string lat { get; set; }
    public string lon { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public int place_rank { get; set; }
    public double importance { get; set; }
    public string addresstype { get; set; }
    public string name { get; set; }
    public string display_name { get; set; }
    public Address address { get; set; }
    public string[] boundingbox { get; set; }
}

public class Address
{
    public string road { get; set; }
    public string neighbourhood { get; set; }
    public string suburb { get; set; }
    public string city { get; set; }
    public string town { get; set; }
    public string village { get; set; }
    public string state { get; set; }
    public string ISO3166_2_lvl4 { get; set; }
    public string postcode { get; set; }
    public string country { get; set; }
    public string country_code { get; set; }
}

