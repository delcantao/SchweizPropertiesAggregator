using Commons;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Scraper.Comparis;
using Scraper.Comparis.Models;
using Scraper.Nominatim;

var id = 1;
var nominatimClient = new NominatimClient();
var geoSwissClient = new GeoSwissClient();
var dbContext = new AppDbContext();
var client = new ComparisClient();
var max = 999;
var Kantons = new List<string>
{
    "Brugg", "Luzern", "Aarau", "Zug", "Glarus", "Appenzell Ausserrhoden", "Appenzell Innerrhoden", "Basel-Landschaft",
    "Basel-Stadt", "Bern", "Freiburg", "Genf", "Glarus", "Jura", "Solothurn", "Tessin", "Thurgau", "Uri", "Valais",
    "Vaud", "Zug", "Winterthur"
};
var prop = new Property();

foreach (var kanton in Kantons)
{
    for (int i = 0; i < max; i++)
    {
        var request = new ComparisRequest
        {
            Page = i,
            SearchParams = new SearchParams
            {
                LocationSearchString = kanton,
                Radius = 1000,
            }
        };
        var res = await client.SearchAsync(request);


        var comparisResponse = res.ToJsonObject<ComparisResponse>();

        max = comparisResponse.TotalPages;


        var list = comparisResponse.ResultItems.Select(e => e.EssentialInformation.Length > 0 ? e.EssentialInformation[0] : string.Empty).Distinct().ToList()
            ;
        
        
        
        foreach (var property in comparisResponse.ResultItems)
        {
            try
            {
                
                var exists = await dbContext.Properties
                    .AnyAsync(x => x.Id == property.AdId);
                if (exists) continue;
                
                var address = string.Join(", ", property.Address);

                var nomRes = await nominatimClient.GetAddress(address);

                if (nomRes == null)
                {
                    var geoRes = await geoSwissClient.GetAddress(address);
                    if (geoRes == null || geoRes.results.Length == 0)
                    {
                        continue;
                        
                    }

                    var erst = geoRes.results[0];
                    
                    nomRes = await nominatimClient.GetAddress($"{geoRes.results[0].attrs.lat},{geoRes.results[0].attrs.lon}");
                     
                    if (nomRes == null)
                    {
                        continue;
                    }
                }
                
             

                var rooms = property.EssentialInformation.Length > 0
                    ? property.EssentialInformation[0].Split(' ')[0]
                    : "0";
                var roomsDouble = Convert.ToDecimal((rooms.OnlyNumbersReturnNumber() == "0"? "0" : rooms), System.Globalization.CultureInfo.InvariantCulture);

                if (roomsDouble > 20)
                {
                    roomsDouble = 0;
                }
                prop = new Property
                {
                    Id = property.AdId,
                    Address = address,
                    Area = Convert.ToDecimal(property.AreaValue * 1.0,
                        System.Globalization.CultureInfo.InvariantCulture),
                    Price = Convert.ToDecimal(property.Price.Replace("'", "").OnlyNumbersReturnNumber(),
                        System.Globalization.CultureInfo.InvariantCulture),
                    Title = property.Title,
                    City = nomRes?.address?.city ?? nomRes?.address?.town ?? nomRes?.address?.village ?? "",
                    // Bathrooms = property.
                    Longitude = Convert.ToDouble(nomRes.lon, System.Globalization.CultureInfo.InvariantCulture),
                    Latitude = Convert.ToDouble(nomRes.lat, System.Globalization.CultureInfo.InvariantCulture),
                    Images = property.ImageUrls.ToList(),
                    Currency = "CHF",
                    Bathrooms = 0,
                    Dealtype = property.DealType,
                    Bedrooms = roomsDouble,
                    Location = new Point(Convert.ToDouble(nomRes.lon), Convert.ToDouble(nomRes.lat))
                    {
                        SRID = 4326
                    },
                    JsonOrig = res,
                    Origin = "comparis",
                    
                };
                dbContext.Properties.Add(prop);
         
                id++;
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException!.Message.Contains("duplicate key"))
                {
                    continue;
                }
                
                Console.WriteLine(prop.ToJson()); 
                Console.WriteLine(e); 
            }
        }


        var file = Path.Combine(ComparisClient.DirectoryResults,
            $"{request.Page + 1}_{request.SearchParams.LocationSearchString}.json");

        if (!File.Exists(file))
        {
            Console.WriteLine("Waiting... 10s");
            await Task.Delay(10000);
        }
    }
}