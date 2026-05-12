using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using HttpClientToCurl.Extensions;
using Scraper.Comparis.Models;

namespace Scraper.Comparis;

public class ComparisClient
{
    private HttpClient _httpClient;
    private const string BaseUrl = "https://www.comparis.ch";

    public const string DirectoryResults =  "/mnt/dados/repos/SchweizPropertiesAggregator/Scraper/results/";
    // "/Users/fedelcantao/repos/PropertiesSchweiz/SchweizPropertiesAgreggator/Scraper/results"; 


    public ComparisClient()
    {
        
        
        var cookieRaw = "AnonymousID=gid=17cedbff-e3d0-4baf-bfdf-4802d5d430fc; UserTrackingID=gid=f69f28fc-d142-4e4f-8085-c02a16e5eb20; datadome=46kJbVQY9MrieeLl6D8oqeqaTi_ixNDefrgN4aNuf3gxlNNpVNrZlw22WnRiNdGIak8z3_xh8zVp_zk9DpJC1ACySUA5y_UtcUPCJdV2Pk_iVFWhf7p4YFwpuII8OdzA; __cmpconsentx102256=CQjuuTAQjuuTAAfEABENCdFoAP_gAEPgAAQ4K5tR_G__bXFu-Tb3abtkeIxX19hr6sAhBgaBsWQFyDuS7JQH12E7JEyKpiYCgRIAu3RBIQNtHBhERUChCIAFJRDMaEGUgDFKIGBkiHERQkMACAwOiIkhWACZYup_NkV5mRqt7ZLu2MzAy5gnr3a5SuQBEJgQCYMNBPhoYBKC87IU12x66wtwsELgAGsOCBnfkGslolcq4tvZZhsQtuSSOCAADAQAEAAAAAAAAAAAAACCuYAJhodEERZEAgQCAhBAgAUFYQAUCAIAAEgaAAAEgYEOQEAFVhMgBACgAGAAEAAIMAAQAACQAYRABAAQCAACAQCAAAAAAACABgYAAwAUIAAAAIDoCIIEAAgWACRCRAaYEIACQQAthAgEAQIK4QoEjgAACIEAgAAAAAKAAAAPCwEJJASsACALAC4ABAAAAAiBBgBSBGAIKAyQYCECTgEjAAAAwAAAAAAAAAAAAAAAAAgAjwAIBrg.IK5tR_G__bXFu-Tb3abtkeIxX19hr6sAhBgaBsWQFyDuS7JQH12E7JEyKpiYCgRIAu3RBIQNtHBhERUChCIAFJRDMaEGUgDFKIGBkiHERQkMACAwOiIkhWACZYup_NkV5mRqt7ZLu2MzAy5gnr3a5SuQBEJgQCYMNBPhoYBKC87IU12x66wtwsELgAGsOCBnfkGslolcq4tvZZhsQtuSSOCAADAQAEAAAAAAAAAAAAAC; __cmpcccx102256=aCQjxnofgA6WMM75jWaezWNYzWOj0Y1OPMWvPGDRw2td5GDGY54ysYMMsta04eTj6M1g1hrHPDTAaYspgsMzGMGRpoy1rDheLUamrSwzVhgZZDAxGaTFgsjFGjDMDTV4-CPGFYBiZFjMGDDE0YWiasaBoYHnjWMMDIxasyatDVkNZdDzA56Y4vSywzQyIMIGllYC0Ed4DIMHF4BqGlgdF6YNWDNGmmicPVgyMsYMWDvWjXB5rLD3mstIxBgsU4HgBIFRIwiglg; _conv_v=vi%3A1*sc%3A16*cs%3A1778566846*fs%3A1771531607*pv%3A127*exp%3A%7B%7D*seg%3A%7B%7D*ps%3A1778531000; _gcl_au=1.1.418041522.1771531608; seerid=bcf19c48-505e-447d-8052-4fa1842f1297; _ga_J1ZTX8T1G0=GS2.1.s1778566846$o24$g1$t1778566942$j60$l0$h0; _ga=GA1.1.1471360630.1771531608; _fbp=fb.1.1771531608159.164798584161041102; _tt_enable_cookie=1; _ttp=01KHVR683V188JH628H22VRHW3_.tt.1; ttcsid_D3VP5NJC77UACP407SG0=1778566847114::EqxYewAx031Xayd7lcr0.21.1778566945461.1; ttcsid=1778566847114::jbU7zYP0QfkB6h6SUBmJ.20.1778566945461.0::1.14103.16746::98346.8.642.1539::91439.4.276; __gads=ID=015724055ad0ff11:T=1771531607:RT=1778566846:S=ALNI_MYp_7WFk2FmPUfJkdQy3SgKSEvj2A; __gpi=UID=000010430b07249f:T=1771531607:RT=1778566846:S=ALNI_MajAstKmIEgRD8mT_Xl91JZUdYhLA; __eoi=ID=f2dc4729fd43d2e6:T=1771531607:RT=1778566846:S=AA-AfjZ3lTijOiz8JCe4CT2hXC75; _clck=126qpqi%5E2%5Eg5z%5E1%5E2241; dakt_2_uuid=8c33aeab7fb785311cca793eff7436b6; dakt_2_uuid_ts=1771531608452; dakt_2_version=3.0.12; permutive-id=9c2e2ca5-faa7-4ba0-955b-f1c40bdce56a; _hjSessionUser_980372=eyJpZCI6ImMwNTg1Njk1LWM5M2MtNWFkNi1iMWJkLTU0MmIxMWM5Y2FmYiIsImNyZWF0ZWQiOjE3NzE1MzE2MTIxOTIsImV4aXN0aW5nIjp0cnVlfQ==; HF_resultpage_searchalert_tooltip=seen; _hjDonePolls=1674642; HF_resultpage_hideusptext=hide; AdvertisementTargetingID=b64d2efe-4fe8-4cbd-6e16-fe35a2d5878b; optimizelyEndUserId=oeu1778411952444r0.1462714807766754; _ga_JRM4RTWQ06=GS2.1.s1778412125$o1$g0$t1778412125$j60$l0$h0; _conv_r=s%3Alocalhost5212*m%3Areferral*t%3A*c%3A; _clsk=1sn1eif%5E1778566864264%5E2%5E0%5Es.clarity.ms%2Fcollect; ASP.NET_SessionId=eznaazhcflgjkwuarkmztss5; website#lang=de-CH; __Secure-Gw-Session=CfDJ8Pak0%2F2mg4NNqzrk0wIau%2Bv7c72TzPWqYa1zGyOk2HgozbvimpnXiSilYWYyDQntQyFxjAHtkc9AxqmOFrxOEGPEOKWqod0QvaAJ%2BZs3B8OIAefkI%2FDlUdxYrEpDWX5gXzPx2Czjb4ohQdB0wMx0%2BsQs12kpq3%2BhvVsdN9mWp9GR; ASP.NET_SessionId.HF=i51akbvyim2yter23iec00fz; UserSessionID=gid=ca96e37f-d142-4cbd-a178-9f0661822f93; _conv_s=sh%3A1778566846270-0.8306200603868621*si%3A16*pv%3A2; seerses=e; dakt_2_session_id=56b2ce017590abd25dd0ed6900f7bb9c; DqSync=true; _rdt_uuid=1771531607986.af792974-292b-46a7-bd93-77478be37151; _uetsid=40a68ba04d7711f182c849499070c8ab; _uetvid=85a284100dce11f1b57df71b2ac2b794; _hjSession_980372=eyJpZCI6IjE1NTg1MjdmLTAzODgtNDk5MS1iMmJhLWI3Y2VlMGY4NjZhOSIsImMiOjE3Nzg1NjY4NjM4NjUsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjowLCJzcCI6MX0=";

        var cookieContainer = new CookieContainer();

        cookieContainer.SetCookies(
            new Uri("https://www.comparis.ch"),
            cookieRaw);

        var handler = new SocketsHttpHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true,

            AllowAutoRedirect = true,

            AutomaticDecompression =
                DecompressionMethods.GZip |
                DecompressionMethods.Deflate |
                DecompressionMethods.Brotli,

            // importante para reutilizar conexão
            PooledConnectionLifetime = TimeSpan.FromMinutes(30),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),

            EnableMultipleHttp2Connections = false
        };

        _httpClient = new HttpClient(handler);

        // HTTP/2
        _httpClient.DefaultRequestVersion = HttpVersion.Version20;

        _httpClient.DefaultVersionPolicy =
            HttpVersionPolicy.RequestVersionOrHigher;

        // HEADERS MÍNIMOS
        // quanto menos inventar, melhor

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (X11; Linux x86_64; rv:147.0) Gecko/20100101 Firefox/147.0");

        _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");

        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
            "en-US,en;q=0.9");

        _httpClient.DefaultRequestHeaders.Referrer =
            new Uri("https://www.comparis.ch/");

        // Firefox normalmente manda isso
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
            "DNT",
            "1");

        // fetch metadata
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
            "Sec-Fetch-Dest",
            "empty");

        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
            "Sec-Fetch-Mode",
            "cors");

        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
            "Sec-Fetch-Site",
            "same-origin");
        
        
    }
    // private void ConfigureHeaders()
    // {
    //     
    //     var uri = new Uri("https://www.comparis.ch");
    //
    //     
    //     _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
    //     _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
    //     _httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
    //     _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64; rv:147.0) Gecko/20100101 Firefox/147.0");
    //     _httpClient.DefaultRequestHeaders.Add("Referer", "https://www.comparis.ch/immobilien/result/list?requestobject=%7B%22DealType%22%3A10%2C%22SiteId%22%3A0%2C%22RootPropertyTypes%22%3A%5B%5D%2C%22PropertyTypes%22%3A%5B%5D%2C%22RoomsFrom%22%3Anull%2C%22RoomsTo%22%3Anull%2C%22FloorSearchType%22%3A0%2C%22LivingSpaceFrom%22%3Anull%2C%22LivingSpaceTo%22%3Anull%2C%22PriceFrom%22%3Anull%2C%22PriceTo%22%3Anull%2C%22ComparisPointsMin%22%3A0%2C%22ShowComparisPoints%22%3Anull%2C%22AdAgeMax%22%3A0%2C%22AdAgeInHoursMax%22%3Anull%2C%22Keyword%22%3A%22%22%2C%22WithImagesOnly%22%3Anull%2C%22WithPointsOnly%22%3Anull%2C%22Radius%22%3Anull%2C%22MinAvailableDate%22%3A%221753-01-01T00%3A00%3A00%22%2C%22MinChangeDate%22%3A%221753-01-01T00%3A00%3A00%22%2C%22LocationSearchString%22%3A%22Luzern%22%2C%22Sort%22%3A3%2C%22HasBalcony%22%3Afalse%2C%22HasTerrace%22%3Afalse%2C%22HasFireplace%22%3Afalse%2C%22HasDishwasher%22%3Afalse%2C%22HasWashingMachine%22%3Afalse%2C%22HasLift%22%3Afalse%2C%22HasParking%22%3Afalse%2C%22PetsAllowed%22%3Afalse%2C%22MinergieCertified%22%3Afalse%2C%22WheelchairAccessible%22%3Afalse%2C%22LowerLeftLatitude%22%3Anull%2C%22LowerLeftLongitude%22%3Anull%2C%22UpperRightLatitude%22%3Anull%2C%22UpperRightLongitude%22%3Anull%2C%22SwapProperty%22%3A1%7D&sort=3");
    //     // _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
    //     _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
    //     _httpClient.DefaultRequestHeaders.Add("dnt", "1");
    //     _httpClient.DefaultRequestHeaders.Add("pragma", "no-cache");
    //     _httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
    //     _httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
    //     _httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
    //     _httpClient.DefaultRequestHeaders.Add("priority", "u=0");
    //     _httpClient.DefaultRequestHeaders.Add("Cookie", cookieRaw);
    //     
    // }

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


        var curlRes = GetViaCurl(url);

        if (curlRes.Contains("AdIdList\":["))
        {
            await File.WriteAllTextAsync(file, curlRes);
            return curlRes;
        }
        else
        {
            throw new Exception($"Erro ao buscar via curl {curlRes}");
        }

    }

    private string GetViaCurl(string url)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "curl",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        psi.ArgumentList.Add(url);

        psi.ArgumentList.Add("--compressed");

        void AddHeader(string header)
        {
            psi.ArgumentList.Add("-H");
            psi.ArgumentList.Add(header);
        }

        AddHeader("User-Agent: Mozilla/5.0 (X11; Linux x86_64; rv:147.0) Gecko/20100101 Firefox/147.0");
        AddHeader("Accept: */*");
        AddHeader("Accept-Language: en-US,en;q=0.9");
        AddHeader("Accept-Encoding: gzip, deflate, br, zstd");

        AddHeader("Referer: https://www.comparis.ch/immobilien/result/list?requestobject=%7B%22DealType%22%3A10%2C%22SiteId%22%3A0%2C%22LocationSearchString%22%3A%22Luzern%22%7D&sort=3");

        AddHeader("Content-Type: application/json");
        AddHeader("Connection: keep-alive");

        psi.ArgumentList.Add("-c");
        psi.ArgumentList.Add("/mnt/dados/repos/SchweizPropertiesAggregator/Scraper/cookies.txt");

        psi.ArgumentList.Add("-b");
        psi.ArgumentList.Add("/mnt/dados/repos/SchweizPropertiesAggregator/Scraper/cookies.txt");
        
        AddHeader("Sec-Fetch-Dest: empty");
        AddHeader("Sec-Fetch-Mode: cors");
        AddHeader("Sec-Fetch-Site: same-origin");

        AddHeader("Priority: u=0");
        AddHeader("Pragma: no-cache");
        AddHeader("Cache-Control: no-cache");

        using var process = Process.Start(psi);

        if (process == null)
            throw new Exception("Falha ao iniciar curl");

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Curl falhou:\n{stderr}");
        }

        return stdout;
    }
}