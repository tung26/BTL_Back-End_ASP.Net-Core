using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface ICommentService
    {
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<IEnumerable<CommentDto>> GetAllCommentAsync();
        Task AddCommentAsync(CommentDto commentDto);
        void UpdateComment(Comment comment);
        void DeleteCommentAsync(int id);
    }
}
