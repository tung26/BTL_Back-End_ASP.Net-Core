using System;
using System.Collections.Generic;

namespace YourHouse.Web.Models.Entities;

public partial class ChungCu
{
    public int ArticleId { get; set; }

    public int? Floor { get; set; }

    public int BedRoom { get; set; }

    public int BathRoom { get; set; }

    public int MaxPerson { get; set; }

    public decimal? WaterPrice { get; set; }

    public decimal? ElectricPrice { get; set; }

    public virtual Article Article { get; set; } = null!;
}
