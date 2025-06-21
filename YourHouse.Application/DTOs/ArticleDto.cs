using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.DTOs
{
    public class ArticleDto
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

        public ChungCuDto? ChungCu { get; set; }

        public HouseDto? House { get; set; }

        public ICollection<ImagesArticleDto> ImagesArticles { get; set; } = new List<ImagesArticleDto>();

        public OfficeDto? Office { get; set; }

        public TroDto? Tro { get; set; }
    }
}
