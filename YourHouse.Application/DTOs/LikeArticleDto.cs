using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class LikeArticleDto
    {
        public int LikeArticleId { get; set; }

        public int ArticleId { get; set; }

        public int AccountId { get; set; }
    }
}
