using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YourHouse.Models.Entities;

public partial class ImagesArticle
{
    public int ImageId { get; set; }

    public int ArticleId { get; set; }

    public string ImageArticle { get; set; } = null!;
    [JsonIgnore]
    public virtual Article Article { get; set; } = null!;
}
