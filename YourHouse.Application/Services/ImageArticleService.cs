using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

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
            var imageArticle = new ImagesArticle() 
            {
                ArticleId = ImageArticleDto.ArticleId,
                ImageArticle = ImageArticleDto.ImageArticle
            };

            await _repository.AddAsync(imageArticle);
            await _repository.SaveChangeAsync();
        }

        public async void DeleteImageArticleAsync(int id)
        {
            var Image = await _repository.GetByIdAsync(id);

            if (Image != null)
            {
                _repository.DeleteAsync(Image);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<ImagesArticleDto>> GetAllImageArticleAsync()
        {
            var imageArticles = await _repository.GetAllAsync();
            return imageArticles.Select(x => new ImagesArticleDto()
            {
                ArticleId = x.ArticleId,
                ImageArticle = x.ImageArticle,
                ImageId = x.ImageId
            });
        }

        public async Task<ImagesArticle?> GetImageArticleByIdAsync(int id)
        {
            var imageArticle = await _repository.GetByIdAsync(id);
            return imageArticle == null ? null : new ImagesArticle()
            {
                ArticleId = imageArticle.ArticleId,
                ImageArticle = imageArticle.ImageArticle,
                ImageId = imageArticle.ImageId
            };
        }

        public async void UpdateImageArticle(ImagesArticle ImageArticle)
        {
            _repository.UpdateAsync(ImageArticle);
            await _repository.SaveChangeAsync();
        }
    }
}
