using System;
using System.Collections.Generic;

namespace YourHouse.Infrastructure;

public partial class Account
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public DateOnly CreateAt { get; set; }

    public string ImageUser { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Role Role { get; set; } = null!;
}
