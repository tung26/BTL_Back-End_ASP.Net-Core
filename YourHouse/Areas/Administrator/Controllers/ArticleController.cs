using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Application.Interfaces;
using YourHouse.Application.Services;
using YourHouse.Web.Controllers;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;

        public ArticleController(IArticleService articleService, ICityService cityService, IDistrictService districtService)
        {
            _articleService = articleService;
            _cityService = cityService;
            _districtService = districtService;
        }

        public async Task<IActionResult> Index()
        {
            if(this.Role == 1 && IsLogin)
            {
                var articleList = await _articleService.GetAllArticleAsync();
                var cities = await _cityService.GetAllCityAsync();
                var districtes = await _districtService.GetAllDistrictAsync();

                var articleListUser = articleList.Select(a => new
                {
                    a.ArticleId,
                    a.TypeAr,
                    a.Title,
                    city = cities.Where(e => e.CityId == a.CityAr).FirstOrDefault().CityName,
                    district = districtes.Where(e => e.DistrictId == a.DistrictAr).FirstOrDefault().DistrictName,
                    a.S,
                    a.Price,
                    a.CreateAt
                });

                ViewData["articleList"] = articleListUser.OrderByDescending(a => a.CreateAt).ToList();

                return View();
            }
            if(IsLogin)
            {
                return RedirectToAction("Index", "MyArticle", new { area = "Administrator" });
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (this.Role == 1 && IsLogin)
            {
                var art = await _articleService.GetArticleByIdAsync(id);

                if (art == null)
                {
                    return RedirectToAction("Index");
                }

                if (art != null)
                {
                    await _articleService.DeleteArticleAsync(id);
                }
                else
                {
                    return Json(new { success = false, message = "Xóa bài đăng thất bại." });
                }

                return Json(new { success = true, message = "Xóa bài đăng thành công!" });
            }

            return RedirectToAction("Index", "Account", new { area = "Adminnistrator" });
        }
    }
}
