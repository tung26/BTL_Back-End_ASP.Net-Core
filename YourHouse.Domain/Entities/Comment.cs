using System;
using System.Collections.Generic;

namespace YourHouse.Web.Infrastructure;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? ParentCommentId { get; set; }

    public int AccountId { get; set; }

    public int ArticleId { get; set; }

    public string Content { get; set; } = null!;

    public DateOnly CreateAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }
}
