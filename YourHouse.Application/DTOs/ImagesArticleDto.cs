using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class ImagesArticleDto
    {
        public int ImageId { get; set; }

        public int ArticleId { get; set; }

        public string ImageArticle { get; set; } = null!;
    }
}
