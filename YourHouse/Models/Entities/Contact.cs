using System;
using System.Collections.Generic;

namespace YourHouse.Models.Entities;

public partial class Contact
{
    public int ContactId { get; set; }

    public int ArticleId { get; set; }

    public int AccountId { get; set; }

    public DateOnly CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
