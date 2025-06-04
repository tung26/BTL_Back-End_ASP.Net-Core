using System;
using System.Collections.Generic;

namespace YourHouse.Models.Entities;

public partial class ImagesArticle
{
    public int ImageId { get; set; }

    public int ArticleId { get; set; }

    public string ImageArticle { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
