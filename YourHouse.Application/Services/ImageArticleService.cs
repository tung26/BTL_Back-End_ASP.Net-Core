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
    public class ImageArticleService : IImageArticleService
    {
        private readonly IRepository<ImagesArticle> _repository;
        public ImageArticleService(IRepository<ImagesArticle> repository)
        {
            _repository = repository;
        }

        public async Task AddImageArticleAsync(ImagesArticleDto ImageArticleDto)
        {
            try
            {
                var imageArticle = new ImagesArticle()
                {
                    ArticleId = ImageArticleDto.ArticleId,
                    ImageArticle = ImageArticleDto.ImageArticle
                };

                await _repository.AddAsync(imageArticle);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteImageArticleAsync(int id)
        {
            try
            {
                var Image = await _repository.GetByIdAsync(id);

                if (Image != null)
                {
                    _repository.DeleteAsync(Image);
                    await _repository.SaveChangeAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ImagesArticleDto>> GetAllImageArticleAsync()
        {
            try
            {
                var imageArticles = await _repository.GetAllAsync();
                return imageArticles.Select(x => new ImagesArticleDto()
                {
                    ArticleId = x.ArticleId,
                    ImageArticle = x.ImageArticle,
                    ImageId = x.ImageId
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ImagesArticle?> GetImageArticleByIdAsync(int id)
        {
            try
            {
                var imageArticle = await _repository.GetByIdAsync(id);
                return imageArticle == null ? null : new ImagesArticle()
                {
                    ArticleId = imageArticle.ArticleId,
                    ImageArticle = imageArticle.ImageArticle,
                    ImageId = imageArticle.ImageId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateImageArticle(ImagesArticle ImageArticle)
        {
            try
            {
                _repository.UpdateAsync(ImageArticle);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
