using System;
using System.Collections.Generic;

namespace YourHouse.Infrastructure;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
