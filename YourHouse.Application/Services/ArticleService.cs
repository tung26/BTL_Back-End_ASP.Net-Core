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
        private readonly IChungCuService _chungCuService;
        private readonly IHouseService _houseService;
        private readonly IOfficeService _officeService;
        private readonly ITroService _troService;
        private readonly IImageArticleService _imageArticleService;

        public ArticleService(
            IRepository<Article> repository, 
            IChungCuService chungCuService, 
            IHouseService houseService, 
            IOfficeService officeService, 
            ITroService troService,
            IImageArticleService imageArticleService
            )
        {
            _repository = repository;
            _chungCuService = chungCuService;
            _houseService = houseService;
            _officeService = officeService;
            _troService = troService;
            _imageArticleService = imageArticleService;
        }

        public async Task AddArticleAsync(ArticleDto articleDto)
        {
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

            switch (article.TypeAr)
            {
                case "Tro":
                    await _troService.DeleteTroAsync(article.AccountId);
                    break;
                case "ChungCu":
                    await _chungCuService.DeleteChungCuAsync(article.AccountId);
                    break;
                case "Office":
                    await _officeService.DeleteOfficeAsync(article.AccountId);
                    break;
                case "House":
                    await _houseService.DeleteHouseAsync(article.AccountId);
                    break;
            }

            //var images = await _imageArticleService.GetAllImageArticleAsync();
            //var imagesDelete = images.Where(i => i.ArticleId == id);

            //foreach (var image in imagesDelete)
            //{
            //    await _imageArticleService.DeleteImageArticleAsync(image.ImageId);
            //}

            if (article != null)
            {
                _repository.DeleteAsync(article);
                await _repository.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticleAsync()
        {
            var articles = await _repository.GetAllAsync();
            var images = await _imageArticleService.GetAllImageArticleAsync();

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
                CreateAt = article.CreateAt,
                ImagesArticles = images.Where(image => image.ArticleId == article.ArticleId).Select(image => image).ToList()
            });
        }

        public async Task<ArticleDto?> GetArticleByIdAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);

            var articleDto = article == null ? null : new ArticleDto()
            {
                AccountId = article.AccountId,
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
            };

            if (articleDto == null )
            {
                return null;
            }

            if (article.TypeAr == "ChungCu")
            {
                var chungCu = await _chungCuService.GetChungCuByIdAsync(id);
                articleDto.ChungCu = chungCu;
            }
            else if (article.TypeAr == "Office")
            {
                var office = await _officeService.GetOfficeByIdAsync(id);
                articleDto.Office = office;
            }
            else if (article.TypeAr == "House")
            {
                var house = await _houseService.GetHouseByIdAsync(id);
                articleDto.House = house;
            }
            else if (article.TypeAr == "Tro")
            {
                var tro = await _troService.GetTroByIdAsync(id);
                articleDto.Tro = tro;
            }

            return articleDto;
        }

        public async void UpdateArticle(ArticleDto articleDto)
        {
            var article = await _repository.GetByIdAsync(articleDto.ArticleId);

            article.Title = articleDto.Title;
            article.DescAr = articleDto.DescAr;
            article.Addr = articleDto.Addr;
            article.CityAr = (int)articleDto.CityAr;
            article.DistrictAr = (int)articleDto.DistrictAr;
            article.S = (decimal)articleDto.S;
            article.Price = (decimal)articleDto.Price;
            article.TienCoc = (decimal)articleDto.TienCoc;
            article.TypeAr = articleDto.TypeAr;

            switch (articleDto.TypeAr)
            {
                case "Tro":
                    {
                        var tro = await _troService.GetTroByIdAsync(article.ArticleId);
                        var troDto = articleDto.Tro;

                        tro.Floor = troDto.Floor;
                        tro.WaterPrice = troDto.WaterPrice;
                        tro.ElectricPrice = troDto.ElectricPrice;
                        tro.MaxPerson = troDto.MaxPerson;

                        break;
                    }
                case "ChungCu":
                    {
                        var cc = await _chungCuService.GetChungCuByIdAsync(article.ArticleId);
                        var ccDto = articleDto.ChungCu;

                        cc.Floor = (int)ccDto.Floor;
                        cc.MaxPerson = (int)ccDto.MaxPerson;
                        cc.WaterPrice = (decimal)ccDto.WaterPrice;
                        cc.ElectricPrice = (decimal)ccDto.ElectricPrice;
                        cc.BedRoom = ccDto.BedRoom;
                        cc.BathRoom = ccDto.BathRoom;
                        break;
                    }
                case "House":
                    {
                        var house = await _houseService.GetHouseByIdAsync(article.ArticleId);
                        var houseDto = articleDto.House;

                        house.Floors = (int)houseDto.Floors;
                        house.BedRoom = (int)houseDto.BedRoom;
                        house.BathRoom = (int)houseDto.BathRoom;

                        break;
                    }
                case "Office":
                    { 
                        var office = await _officeService.GetOfficeByIdAsync(article.ArticleId);
                        var officeDto = articleDto.Office;

                        office.Floor = officeDto.Floor;
                        office.DoorDrt = (int)officeDto.DoorDrt;

                        break;
                    } 
            }

            _repository.UpdateAsync(article);
            await _repository.SaveChangeAsync();
        }
    }
}
