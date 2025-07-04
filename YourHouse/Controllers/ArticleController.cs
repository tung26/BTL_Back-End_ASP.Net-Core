using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Application.Services;
using YourHouse.Infrastructure;
using YourHouse.Infrastructure.Data;
using YourHouse.Web.Models;


namespace YourHouse.Web.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ITroService _troService;
        private readonly IChungCuService _chungCuService;
        private readonly IHouseService _houseService;
        private readonly IOfficeService _officeService;
        private readonly IImageArticleService _imageArticleService;
        private readonly ICommentService _commentService;
        private readonly ILikeArticleService _likeArticleService;

        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;
        private readonly IAccountService _accountService;

        public ArticleController(IArticleService articleService, 
            IDistrictService districtService, 
            ICityService cityService, 
            IAccountService accountService, 
            ITroService troService, 
            IChungCuService chungCuService, 
            IHouseService houseService, 
            IOfficeService officeService,
            IImageArticleService imageArticleService,
            ICommentService commentService,
            ILikeArticleService likeArticleService
            )
        {
            _articleService = articleService;
            _cityService = cityService;
            _districtService = districtService;
            _accountService = accountService;
            _troService = troService;
            _chungCuService = chungCuService;
            _houseService = houseService;
            _officeService = officeService;
            _imageArticleService = imageArticleService;
            _commentService = commentService;
            _likeArticleService = likeArticleService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["City"] = (await _cityService.GetAllCityAsync()).ToList();
            ViewData["District"] = (await _districtService.GetAllDistrictAsync()).Select(d => new DistrictDto { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);

            var likeArticle = (await _likeArticleService.GetAllLikeArticleAsync()).Where(l => l.ArticleId == id && l.AccountId == this.IdUser).FirstOrDefault();
            
            switch(article.TypeAr)
            {
                case "Tro":
                    {
                        var tro = await _troService.GetTroByIdAsync(id);
                        article.Tro = tro;
                        break;
                    }
                case "ChungCu":
                    {
                        var chungCu = await _chungCuService.GetChungCuByIdAsync(id);
                        article.ChungCu = chungCu;
                        break;
                    }
                case "House":
                    {
                        var house = await _houseService.GetHouseByIdAsync(id);
                        article.House = house;
                        break;
                    }
                case "Office":
                    {
                        var office = await _officeService.GetOfficeByIdAsync(id);
                        article.Office = office;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var imgs = await _imageArticleService.GetAllImageArticleAsync();
            var imagesArticles = imgs.Where(i => i.ArticleId == id);

            foreach(var img in imagesArticles)
            {
                article.ImagesArticles.Add(img);
            }

            var acc = await _accountService.GetAccountByIdAsync(article.AccountId);
            var city = (await _cityService.GetCityByIdAsync(article.CityAr)).CityName;
            var district = (await _districtService.GetDistrictByIdAsync(article.DistrictAr)).DistrictName;
            var comments = (await _commentService.GetAllCommentAsync()).Where(x => x.ArticleId == id).OrderBy(x => x.CreateAt).ToList();

            var accounts = await _accountService.GetAllAccountAsync();

            foreach (var comment in comments)
            {
                var account = accounts.Where(x => x.AccountId == comment.AccountId).FirstOrDefault();

                comment.ImageUser = account.ImageUser;
                comment.UserName = account.FullName;
            }

            ViewBag.comments = comments;
            ViewBag.Acc = acc;
            ViewBag.City = city;
            ViewBag.District = district;
            ViewBag.isLike = false;

            if (likeArticle != null)
            {
                ViewBag.isLike = true;
            }

            return View(article);
        }

        public async Task<IActionResult> ReportArticle(int id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdAsync(id);
                article.StatusAr = 0;
                await _articleService.UpdateArticle(article);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        public async Task<IActionResult> Filters(string type, int city, int district, decimal? min, decimal? max)
        {
            var result = await _articleService.GetAllArticleAsync();
            result = result.ToList();

            var images = await _imageArticleService.GetAllImageArticleAsync();

            foreach (var r in result)
            {
                var imgs = images.Where(i => r.ArticleId == i.ArticleId).Select(e => e).ToList();
                foreach (var img in imgs)
                {
                    r.ImagesArticles.Add(img);
                }
            }

            if (type != "All")
            {
                result = result.Where(a => a.TypeAr == type).Select(a => a);
            }
            if(city != -1)
            {
                result = result.Where(a => a.CityAr == city).Select(a => a);
            }
            if(district != -1)
            {
                result = result.Where(a => a.DistrictAr == district).Select(a => a);
            }
            if(min != null && max != null)
            {
                result = result.Where(a => a.Price >= min && a.Price <= max).Select(a => a);
            }

            return Ok(result.ToList());
        }
    }
}
