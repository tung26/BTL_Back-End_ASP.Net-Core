using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IArticleService
    {
        Task<ArticleDto?> GetArticleByIdAsync(int id);
        Task<IEnumerable<ArticleDto>> GetAllArticleAsync();
        Task AddArticleAsync(ArticleDto articleDto);
        void UpdateArticle(ArticleDto articleDto);
        Task DeleteArticleAsync(int id);
    }
}
