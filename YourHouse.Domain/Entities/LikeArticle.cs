using System;
using System.Collections.Generic;

namespace YourHouse.Infrastructure;

public partial class LikeArticle
{
    public int LikeArticleId { get; set; }

    public int ArticleId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
