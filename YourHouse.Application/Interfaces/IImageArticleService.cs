using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Interfaces
{
    public interface IImageArticleService
    {
        Task<ImagesArticle?> GetImageArticleByIdAsync(int id);
        Task<IEnumerable<ImagesArticleDto>> GetAllImageArticleAsync();
        Task AddImageArticleAsync(ImagesArticleDto ImageArticleDto);
        Task UpdateImageArticle(ImagesArticle ImageArticle);
        Task DeleteImageArticleAsync(int id);
    }
}
