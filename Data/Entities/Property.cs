using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Data.Entities;

public partial class Property
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public double Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    public decimal Area { get; set; }

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public Point Location { get; set; } = null!;

    public List<string> Images { get; set; } = null!;

    public string JsonOrig { get; set; } = null!; 
}
