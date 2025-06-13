using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YourHouse.Models.Entities;

public partial class Article
{
    public int ArticleId { get; set; }

    public int AccountId { get; set; }

    public string Title { get; set; } = null!;

    public string DescAr { get; set; } = null!;

    public string Addr { get; set; } = null!;

    public int CityAr { get; set; }

    public int DistrictAr { get; set; }

    public decimal S { get; set; }

    public decimal Price { get; set; }

    public decimal? TienCoc { get; set; }

    public string TypeAr { get; set; } = null!;

    public int StatusAr { get; set; }

    public DateOnly CreateAt { get; set; }

    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;

    public virtual ChungCu? ChungCu { get; set; }

    public virtual City CityArNavigation { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual District DistrictArNavigation { get; set; } = null!;

    public virtual House? House { get; set; }

    public virtual ICollection<ImagesArticle> ImagesArticles { get; set; } = new List<ImagesArticle>();

    public virtual Office? Office { get; set; }

    public virtual Tro? Tro { get; set; }
}
