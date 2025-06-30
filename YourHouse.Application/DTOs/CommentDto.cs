using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }

        public int? ParentCommentId { get; set; }

        public int AccountId { get; set; }

        public int ArticleId { get; set; }

        public string? ImageUser { get; set; }
        public string? UserName { get; set; }
        public bool IsDelete { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống bình luận.")]
        public string Content { get; set; } = null!;


        public DateOnly CreateAt { get; set; }
    }
}
