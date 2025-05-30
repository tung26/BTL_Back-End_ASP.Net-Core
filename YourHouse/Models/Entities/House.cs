using System;
using System.Collections.Generic;

namespace YourHouse.Models.Entities;

public partial class House
{
    public int ArticleId { get; set; }

    public int BedRoom { get; set; }

    public int BathRoom { get; set; }

    public int Floors { get; set; }

    public virtual Article Article { get; set; } = null!;
}
