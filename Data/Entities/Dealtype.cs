using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Dealtype
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
