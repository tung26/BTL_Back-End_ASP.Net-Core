using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Services
{
    public class LikeArticleService : ILikeArticleService
    {
        private readonly IRepository<LikeArticle> _repository;

        public async Task AddLikeArticleAsync(LikeArticleDto likeArticleDto)
        {
            var likeArticle = new LikeArticle()
            {
                ArticleId = likeArticleDto.ArticleId,
                AccountId = likeArticleDto.AccountId,
            };

            await _repository.AddAsync(likeArticle);
            await _repository.SaveChangeAsync();
        }

        public async Task DeleteLikeArticleAsync(int id)
        {
            var likeArticle = await _repository.GetByIdAsync(id);
            _repository.DeleteAsync(likeArticle);
            await _repository.SaveChangeAsync();
        }

        public async Task<IEnumerable<LikeArticleDto>> GetAllLikeArticleAsync()
        {
            var likeArtiles = await _repository.GetAllAsync();

            return likeArtiles.Select(x => new LikeArticleDto()
            {
                LikeArticleId = x.LikeArticleId,
                ArticleId = x.ArticleId,
                AccountId = x.AccountId,
            });
        }

        public async Task<LikeArticleDto?> GetLikeArticleByIdAsync(int id)
        {
            var likeArticle = await _repository.GetByIdAsync(id);

            return likeArticle == null ? null : new LikeArticleDto()
            {
                LikeArticleId = likeArticle.LikeArticleId,
                ArticleId = likeArticle.ArticleId,
                AccountId = likeArticle.AccountId,
            };
        }

        public async Task UpdateLikeArticle(LikeArticleDto likeArticleDto)
        {
            var likeArticle = await _repository.GetByIdAsync(likeArticleDto.LikeArticleId);

            likeArticle.ArticleId = likeArticleDto.ArticleId;
            likeArticle.AccountId = likeArticleDto.AccountId;

            _repository.UpdateAsync(likeArticle);
            await _repository.SaveChangeAsync();
        }
    }
}
