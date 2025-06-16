using System;
using System.Collections.Generic;

namespace YourHouse.Web.Infrastructure;

public partial class District
{
    public int CityId { get; set; }

    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual City City { get; set; } = null!;
}
