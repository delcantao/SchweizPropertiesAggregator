using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Scraper.Comparis.Models;

namespace Scraper.Comparis;


public class ComparisClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://www.comparis.ch";
    public const string DirectoryResults = "/mnt/dados/repos/SchweizPropertiesAggregator/Scraper/results/";
    
        
    public ComparisClient()
    {
        var handler = new HttpClientHandler
        {
            AutomaticDecompression =
                DecompressionMethods.GZip |
                DecompressionMethods.Deflate |
                DecompressionMethods.Brotli,
            
            CookieContainer = new CookieContainer()
        };

 
        var cookies = new CookieContainer();
        var uri = new Uri(BaseUrl);
        
        
        cookies.Add(uri, new Cookie("ASP.NET_SessionId", "k0ovbhnxoqtw1fblbmv53cu5"));
        cookies.Add(uri, new Cookie("AnonymousID", "gid=17cedbff-e3d0-4baf-bfdf-4802d5d430fc"));
        cookies.Add(uri, new Cookie("UserTrackingID", "gid=f69f28fc-d142-4e4f-8085-c02a16e5eb20"));

        cookies.Add(uri, new Cookie(
            "datadome",
            "dvBCah06tMJmii7VKwYU4qrMaL5YOKiETyha~yqouSu1QlZTmC2CPNiEOCsUghQ_39d3DD5tBhunwZqbikoxmzzWuZ~d9NWHWUpMZ2oOvRJRD7tk5Y0PwGTn5dvL77S2"
        ));

        cookies.Add(uri, new Cookie(
            "__cmpconsentx102256",
            "CQjuuTAQjuuTAAfEABENCdFoAP_gAEPgAAQ4K5tR_G__bXFu-Tb3abtkeIxX19hr6sAhBgaBsWQFyDuS7JQH12E7JEyKpiYCgRIAu3RBIQNtHBhERUChCIAFJRDMaEGUgDFKIGBkiHERQkMACAwOiIkhWACZYup_NkV5mRqt7ZLu2MzAy5gnr3a5SuQBEJgQCYMNBPhoYBKC87IU12x66wtwsELgAGsOCBnfkGslolcq4tvZZh"
        ));
        
        cookies.Add(uri, new Cookie("ASP.NET_SessionId.HF", "1uispg2xmf1zcsuelj1d3rn4"));
        cookies.Add(uri, new Cookie("website#lang", "de-CH"));

        cookies.Add(uri, new Cookie(
            "_clsk",
            "11n6iim^1778356906483^9^0^s.clarity.ms/collect"
        ));

        cookies.Add(uri, new Cookie(
            "_conv_s",
            "sh:1778252958017-0.868585603299418*si:11*pv:18"
        ));

        cookies.Add(uri, new Cookie("seerses", "e"));
        cookies.Add(uri, new Cookie("dakt_2_session_id", "0667a10b1c1b09ebfb0a36a384244630"));
        cookies.Add(uri, new Cookie("DqSync", "true"));

        cookies.Add(uri, new Cookie(
            "UserSessionID",
            "gid=05cee352-51a3-475d-8005-017f21f64fb5"
        ));

        cookies.Add(uri, new Cookie(
            "_rdt_uuid",
            "1771531607986.af792974-292b-46a7-bd93-77478be37151"
        ));

        cookies.Add(uri, new Cookie("_uetsid", "4d333b904be011f1bc9d31ae22546114"));
        cookies.Add(uri, new Cookie("_uetvid", "85a284100dce11f1b57df71b2ac2b794"));


        handler.CookieContainer = cookies;

            

        _httpClient = new HttpClient(handler);

        ConfigureHeaders();
    }

    private void ConfigureHeaders()
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("*/*"));

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(
            new StringWithQualityHeaderValue("de-CH"));

        _httpClient.DefaultRequestHeaders.CacheControl =
            new CacheControlHeaderValue
            {
                NoCache = true
            };

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/147.0.0.0 Safari/537.36"
        );

        _httpClient.DefaultRequestHeaders.Referrer =
            new Uri("https://www.comparis.ch");

        _httpClient.DefaultRequestHeaders.Add("dnt", "1");
        _httpClient.DefaultRequestHeaders.Add("pragma", "no-cache");

        _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
        _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        
    }

    public async Task<string> SearchAsync(ComparisRequest request)
    {

        var file = Path.Combine(ComparisClient.DirectoryResults,
            $"{request.Page}_{request.SearchParams.LocationSearchString}.json"); 

        if (File.Exists(file))
        {
            return await File.ReadAllTextAsync(file);
        }

        var encodedRequest = request.ToUrlEncodedJson();
        var url =
            "https://www.comparis.ch/immobilien/api/v1/singlepage/resultitems" +
            $"?requestObject={encodedRequest}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var res = await response.Content.ReadAsStringAsync();
        await File.WriteAllTextAsync(file, res);
        return res;
        
    }
}
