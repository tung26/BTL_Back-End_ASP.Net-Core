using System;
using System.Collections.Generic;

namespace YourHouse.Models.Entities;

public partial class Tro
{
    public int ArticleId { get; set; }

    public int? Floor { get; set; }

    public int MaxPerson { get; set; }

    public decimal? WaterPrice { get; set; }

    public decimal? ElectricPrice { get; set; }

    public virtual Article Article { get; set; } = null!;
}
