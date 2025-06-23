using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Services
{
    public class ArticleService : IArticleService
    {

        private readonly IRepository<Article> _repository;

        public ArticleService(
            IRepository<Article> repository
            )
        {
            _repository = repository;
        }

        public async Task AddArticleAsync(ArticleDto articleDto)
        {
            var images = new List<ImagesArticle>();
            var imagesDto = articleDto.ImagesArticles;

            foreach ( var image in imagesDto )
            {
                images.Add(new ImagesArticle()
                {
                    ArticleId = image.ArticleId,
                    ImageArticle = image.ImageArticle,
                });
            }

            var article = new Article()
            {
                AccountId = articleDto.AccountId,
                Title = articleDto.Title,
                DescAr = articleDto.DescAr,
                Addr = articleDto.Addr,
                CityAr = articleDto.CityAr,
                DistrictAr = articleDto.DistrictAr,
                S = articleDto.S,
                Price = articleDto.Price,
                TienCoc = articleDto.TienCoc,
                TypeAr = articleDto.TypeAr,
                StatusAr = articleDto.StatusAr,
                ImagesArticles = images,
            };

            if (articleDto.TypeAr == "ChungCu")
            {
                ChungCuDto ccDto = articleDto.ChungCu;
                ChungCu chungCu = new ChungCu()
                {
                    Floor = ccDto.Floor,
                    BedRoom = ccDto.BedRoom,
                    BathRoom = ccDto.BedRoom,
                    MaxPerson = ccDto.MaxPerson,
                    WaterPrice = ccDto.WaterPrice,
                    ElectricPrice = ccDto.ElectricPrice,
                };

                article.ChungCu = chungCu;
            }
            else if (articleDto.TypeAr == "Office")
            {
                OfficeDto officeDto = articleDto.Office;
                Office office = new Office()
                {
                    Floor = officeDto.Floor,
                    DoorDrt = officeDto.DoorDrt,
                };

                article.Office = office;
            }
            else if (articleDto.TypeAr == "House")
            {
                HouseDto houseDto = articleDto.House;
                House house = new House()
                {
                    Floors = houseDto.Floors,
                    BedRoom = houseDto.BedRoom,
                    BathRoom= houseDto.BedRoom,
                };

                article.House = house;
            }
            else if (articleDto.TypeAr == "Tro")
            {
                TroDto troDto = articleDto.Tro;
                Tro tro = new Tro()
                {
                    Floor = troDto.Floor,
                    MaxPerson = troDto.MaxPerson,
                    WaterPrice = troDto.WaterPrice,
                    ElectricPrice= troDto.ElectricPrice,
                };

                article.Tro = tro;
            }
            else
            {
                throw new Exception();
            }

            await _repository.AddAsync(article);
            await _repository.SaveChangeAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);

            if (article != null)
            {
                _repository.DeleteAsync(article);
                await _repository.SaveChangeAsync();
            }
        }   

        public async Task<IEnumerable<ArticleDto>> GetAllArticleAsync()
        {
            var articles = await _repository.GetAllAsync();

            return articles.Select(article => new ArticleDto()
            {
                AccountId = article.AccountId,
                ArticleId = article.ArticleId,
                Title = article.Title,
                DescAr = article.DescAr,
                Addr = article.Addr,
                CityAr = article.CityAr,
                DistrictAr = article.DistrictAr,
                S = article.S,
                Price = article.Price,
                TienCoc = article.TienCoc,
                TypeAr = article.TypeAr,
                StatusAr = article.StatusAr,
                CreateAt = article.CreateAt
            });
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);

            var articleDto = article == null ? null : new ArticleDto()
            {
                AccountId = article.AccountId,
                ArticleId = article.ArticleId,
                Title = article.Title,
                DescAr = article.DescAr,
                Addr = article.Addr,
                CityAr = article.CityAr,
                DistrictAr = article.DistrictAr,
                S = article.S,
                Price = article.Price,
                TienCoc = article.TienCoc,
                TypeAr = article.TypeAr,
                StatusAr = article.StatusAr,
                CreateAt = article.CreateAt,
                ImagesArticles = new List<ImagesArticleDto>()
            };

            if (articleDto == null )
            {
                return null;
            }

            return articleDto;
        }

        public async Task UpdateArticle(ArticleDto articleDto)
        {
            var article = await _repository.GetByIdAsync(articleDto.ArticleId);

            if (article != null)
            {
                article.Title = articleDto.Title;
                article.DescAr = articleDto.DescAr;
                article.Addr = articleDto.Addr;
                article.CityAr = (int)articleDto.CityAr;
                article.DistrictAr = (int)articleDto.DistrictAr;
                article.S = (decimal)articleDto.S;
                article.Price = (decimal)articleDto.Price;
                article.TienCoc = (decimal)articleDto.TienCoc;
                article.TypeAr = articleDto.TypeAr;

                _repository.UpdateAsync(article);
                await _repository.SaveChangeAsync();
            }
        }
    }
}
