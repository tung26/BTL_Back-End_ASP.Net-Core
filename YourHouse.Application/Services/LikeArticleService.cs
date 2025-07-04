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

        public LikeArticleService(IRepository<LikeArticle> repository)
        {
            _repository = repository;
        }

        public async Task AddLikeArticleAsync(LikeArticleDto likeArticleDto)
        {
            try
            {
                var likeArticle = new LikeArticle()
                {
                    ArticleId = likeArticleDto.ArticleId,
                    AccountId = likeArticleDto.AccountId,
                };

                await _repository.AddAsync(likeArticle);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteLikeArticleAsync(int id)
        {
            try
            {
                var likeArticle = await _repository.GetByIdAsync(id);
                _repository.DeleteAsync(likeArticle);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LikeArticleDto>> GetAllLikeArticleAsync()
        {
            try
            {
                var likeArtiles = await _repository.GetAllAsync();

                return likeArtiles.Select(x => new LikeArticleDto()
                {
                    LikeArticleId = x.LikeArticleId,
                    ArticleId = x.ArticleId,
                    AccountId = x.AccountId,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LikeArticleDto?> GetLikeArticleByIdAsync(int id)
        {
            try
            {
                var likeArticle = await _repository.GetByIdAsync(id);

                return likeArticle == null ? null : new LikeArticleDto()
                {
                    LikeArticleId = likeArticle.LikeArticleId,
                    ArticleId = likeArticle.ArticleId,
                    AccountId = likeArticle.AccountId,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateLikeArticle(LikeArticleDto likeArticleDto)
        {
            try
            {
                var likeArticle = await _repository.GetByIdAsync(likeArticleDto.LikeArticleId);

                likeArticle.ArticleId = likeArticleDto.ArticleId;
                likeArticle.AccountId = likeArticleDto.AccountId;

                _repository.UpdateAsync(likeArticle);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
