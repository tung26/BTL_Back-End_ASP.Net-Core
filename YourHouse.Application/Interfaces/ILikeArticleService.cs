using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface ILikeArticleService
    {
        Task<LikeArticleDto?> GetLikeArticleByIdAsync(int id);
        Task<IEnumerable<LikeArticleDto>> GetAllLikeArticleAsync();
        Task AddLikeArticleAsync(LikeArticleDto likeArticleDto);
        Task UpdateLikeArticle(LikeArticleDto likeArticleDto);
        Task DeleteLikeArticleAsync(int id);
    }
}
