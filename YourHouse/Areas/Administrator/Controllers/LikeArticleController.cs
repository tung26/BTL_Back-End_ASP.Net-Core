using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Infrastructure;
using YourHouse.Web.Controllers;

namespace YourHouse.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class LikeArticleController : BaseController
    {
        private readonly ILikeArticleService _likeArticleService;
        private readonly IArticleService _articleService;
        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;
        
        public LikeArticleController(ILikeArticleService likeArticleService, IArticleService articleService, ICityService cityService, IDistrictService districtService)
        {
            _likeArticleService = likeArticleService;
            _articleService = articleService;
            _cityService = cityService;
            _districtService = districtService;
        }

        public async Task<IActionResult> Index()
        {
            if(this.IsLogin)
            {
                var articles = await _articleService.GetAllArticleAsync();
                var artilcesLike = await _likeArticleService.GetAllLikeArticleAsync();
                var cities = await _cityService.GetAllCityAsync();
                var districts = await _districtService.GetAllDistrictAsync();
                var listArticlesId = artilcesLike.Where(a => a.AccountId == this.IdUser).Select(x => x.ArticleId);

                var listArticleLike = articles
                    .Where(a => listArticlesId.Contains(a.ArticleId))
                    .Select(a => new
                    {
                        a.ArticleId,
                        a.TypeAr,
                        a.Title,
                        city = cities.Where(e => e.CityId == a.CityAr).FirstOrDefault().CityName,
                        district = districts.Where(e => e.DistrictId == a.DistrictAr).FirstOrDefault().DistrictName,
                        a.S,
                        a.Price,
                        a.CreateAt
                    })
                    .ToList();

                ViewData["articleList"] = listArticleLike;
                return View();
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        public async Task<IActionResult> Add(int id)
        {
            Console.WriteLine(id);
            if(this.IsLogin)
            {
                var likeArticleDto = new LikeArticleDto()
                {
                    ArticleId = id,
                    AccountId = (int)this.IdUser
                };

                await _likeArticleService.AddLikeArticleAsync(likeArticleDto);

                return Json(new { success = true, message = "Đã thêm vào mục yêu thích." });
            }
            return Json(new { success = false, message = "Yêu cầu đăng nhập." });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.IsLogin)
            {
                var likeArticle = (await _likeArticleService.GetAllLikeArticleAsync()).Where(l => l.ArticleId == id && l.AccountId == this.IdUser).FirstOrDefault();

                if (likeArticle != null)
                {
                    await _likeArticleService.DeleteLikeArticleAsync(likeArticle.LikeArticleId);
                    return Json(new { success = true, message = "Đã xóa khỏi mục yêu thích." });
                }
                return Json(new { success = false, message = "Lỗi." });
            }
            return Json(new { success = false, message = "Yêu cầu đăng nhập." });
        }
    }
}
