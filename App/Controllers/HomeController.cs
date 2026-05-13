using Commons;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;


public class HomeController(AppDbContext dbContext) : Controller
{
    public IActionResult Index()
    {
        return View();
    }


    [HttpGet("api/properties")]
    public async Task<IActionResult> Get(
        double west,
        double south,
        double east,
        double north)
    {
        var properties = await dbContext.Properties
            .Where(x =>
                x.Longitude >= west &&
                x.Longitude <= east &&
                x.Latitude >= south &&
                x.Latitude <= north)
            .Take(500)
            .ToListAsync();

        return Json(properties);
    }

    [HttpGet]
    public IActionResult Time()
    {
        return PartialView("_TimePartial", DateTime.Now);
    }

    [HttpGet("properties/cards")]
    public async Task<IActionResult> Cards(
        double west,
        double south,
        double east,
        double north,
        double? minArea = null,
        int? minBedrooms = null,
        int? dealtype = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var query = dbContext.Properties
            .Include(x => x.DealtypeNavigation)
            .Where(x =>
                x.Longitude >= west &&
                x.Longitude <= east &&
                x.Latitude >= south &&
                x.Latitude <= north);

        if (minArea.HasValue)
            query = query.Where(x => (double) x.Area >= minArea.Value);

        if (minBedrooms.HasValue)
            query = query.Where(x => (int) x.Bedrooms >= minBedrooms.Value);

        if (dealtype.HasValue)
            query = query.Where(x => x.Dealtype == dealtype.Value);

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice.Value);

        var properties = await query
            .OrderBy(x => x.Price)
            .Take(50).ToListAsync();

        return PartialView("_CardsPartial", properties);
    }


    [HttpGet("api/properties/map")]
    public async Task<IActionResult> Map(
        double west,
        double south,
        double east,
        double north,
        double? minArea = null,
        int? minBedrooms = null,
        int? dealtype = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var query = dbContext.Properties
            .Where(x =>
                x.Longitude >= west &&
                x.Longitude <= east &&
                x.Latitude >= south &&
                x.Latitude <= north);

        if (minArea.HasValue)
            query = query.Where(x => (double) x.Area >= minArea.Value);

        if (minBedrooms.HasValue)
            query = query.Where(x => (int) x.Bedrooms >= minBedrooms.Value);

        if (dealtype.HasValue)
            query = query.Where(x => x.Dealtype == dealtype.Value);

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice.Value);

        var properties = await query.Take(1000).ToListAsync();

        // foreach (var property in properties)
        // {
        //     try
        //     {
        //         property.Images = property.Images.ToJsonObject<List<string>>()!.FirstOrDefault() ?? ;
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //     }
        // }


        var geojson = new
        {
            type = "FeatureCollection",

            features = properties.Select(x => new
            {
                type = "Feature",

                geometry = new
                {
                    type = "Point",

                    coordinates = new[]
                    {
                        x.Longitude,
                        x.Latitude
                    }
                },

                properties = new
                {
                    id = x.Id,
                    title = x.Title,
                    price = x.Price,
                    image = x.Images
                }
            })
        };

        return Json(geojson);
    }
}