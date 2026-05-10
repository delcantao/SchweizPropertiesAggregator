using System.Text.Encodings.Web;
using System.Text.Json;

namespace Scraper.Comparis.Models;

public class ComparisRequest
{
    public Header Header { get; set; } = new();
    public SearchParams SearchParams { get; set; } = new();
    public int Page { get; set; }

    public string ToUrlEncodedJson()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = false,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        return Uri.EscapeDataString(json);
    }
}

public class Header
{
    public string Language { get; set; } = "de";
}

public class SearchParams
{
    public int DealType { get; set; } = 20;
    public int SwapProperty { get; set; } = 1;
    public int SiteId { get; set; } = 0;

    public List<int> RootPropertyTypes { get; set; } = [];
    public List<int> PropertyTypes { get; set; } = [];

    public int? RoomsFrom { get; set; }
    public int? RoomsTo { get; set; }

    public int FloorSearchType { get; set; } = 0;

    public double? LivingSpaceFrom { get; set; }
    public double? LivingSpaceTo { get; set; }

    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }

    public int ComparisPointsMin { get; set; } = 0;

    public int AdAgeMax { get; set; } = 0;
    public int? AdAgeInHoursMax { get; set; }

    public string Keyword { get; set; } = "";

    public bool? WithImagesOnly { get; set; }
    public bool? WithPointsOnly { get; set; }

    public int? Radius { get; set; }

    public DateTime MinAvailableDate { get; set; } =
        new(1753, 1, 1);

    public DateTime MinChangeDate { get; set; } =
        new(1753, 1, 1);

    public string LocationSearchString { get; set; } = "Luzern";

    public int Sort { get; set; } = 3;

    public bool HasBalcony { get; set; }
    public bool HasTerrace { get; set; }
    public bool HasFireplace { get; set; }
    public bool HasDishwasher { get; set; }
    public bool HasWashingMachine { get; set; }
    public bool HasLift { get; set; }
    public bool HasParking { get; set; }
    public bool PetsAllowed { get; set; }
    public bool MinergieCertified { get; set; }
    public bool WheelchairAccessible { get; set; }

    public double? LowerLeftLatitude { get; set; }
    public double? LowerLeftLongitude { get; set; }

    public double? UpperRightLatitude { get; set; }
    public double? UpperRightLongitude { get; set; }
}