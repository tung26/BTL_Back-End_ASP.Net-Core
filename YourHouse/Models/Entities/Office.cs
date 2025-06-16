using System;
using System.Collections.Generic;

namespace YourHouse.Web.Models.Entities;

public partial class Office
{
    public int ArticleId { get; set; }

    public int? Floor { get; set; }

    public int? DoorDrt { get; set; }

    public virtual Article Article { get; set; } = null!;
}
