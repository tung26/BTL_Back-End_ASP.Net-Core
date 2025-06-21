using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Application.Services;
using YourHouse.Web.Infrastructure;
using YourHouse.Web.Infrastructure.Data;
using YourHouse.Web.Models;


namespace YourHouse.Web.Controllers
{
    public class ArticleController : BaseController
    {
        //private readonly YourHouseContext _context;
        private readonly IArticleService _articleService;
        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;
        private readonly IAccountService _accountService;

        public ArticleController(IArticleService articleService, IDistrictService districtService, ICityService cityService, IAccountService accountService)
        {
            _articleService = articleService;
            _cityService = cityService;
            _districtService = districtService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["City"] = (await _cityService.GetAllCityAsync()).ToList();
            ViewData["District"] = (await _districtService.GetAllDistrictAsync()).Select(d => new DistrictDto { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            //var articleList = _articleService.GetAllArticleAsync();

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);

            //if (article == null)
            //{
            //    return NotFound();
            //}

            var acc = await _accountService.GetAccountByIdAsync(article.AccountId);
            var city = (await _cityService.GetCityByIdAsync(article.CityAr)).CityName;
            var district = (await _districtService.GetDistrictByIdAsync(article.DistrictAr)).DistrictName;

            ViewBag.Acc = acc;
            ViewBag.City = city;
            ViewBag.District = district;

            //return View();

            return View(article);
        }

        public async Task<IActionResult> Filters(string type, int city, int district, decimal? min, decimal? max)
        {
            var result = await _articleService.GetAllArticleAsync();

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
