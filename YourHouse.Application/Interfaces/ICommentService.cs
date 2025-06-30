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
        Task<CommentDto?> GetCommentByIdAsync(int id);
        Task<IEnumerable<CommentDto>> GetAllCommentAsync();
        Task<int> AddCommentAsync(CommentDto commentDto);
        Task UpdateComment(CommentDto commentDto);
        Task DeleteCommentAsync(int id, bool articleDelete = false);
    }
}
