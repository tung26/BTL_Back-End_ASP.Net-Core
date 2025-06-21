using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }

        public int? ParentCommentId { get; set; }

        public int AccountId { get; set; }

        public int ArticleId { get; set; }

        public string Content { get; set; } = null!;

        public DateOnly CreateAt { get; set; }
    }
}
