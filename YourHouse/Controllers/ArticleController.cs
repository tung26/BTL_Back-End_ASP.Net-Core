using Microsoft.AspNetCore.Mvc;
using YourHouse.Models.Entities;

namespace YourHouse.Controllers
{
    public class ArticleController : Controller
    {
        private readonly YourHouseContext _context;

        public ArticleController(YourHouseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["City"] = _context.Cities.ToList();
            ViewData["District"] = _context.Districts.Select(d => new District { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            var articleList = _context.Articles.Select(e => e).ToList();
            return View(articleList);
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
