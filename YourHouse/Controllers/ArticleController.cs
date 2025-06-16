using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourHouse.Models;
using YourHouse.Models.Entities;

namespace YourHouse.Controllers
{
    public class ArticleController : BaseController
    {
        //private readonly YourHouseContext _context;

        public ArticleController(YourHouseContext context) : base(context) { }

        public IActionResult Index()
        {
            ViewData["City"] = _context.Cities.ToList();
            ViewData["District"] = _context.Districts.Select(d => new District { DistrictId = d.DistrictId, DistrictName = d.DistrictName, CityId = d.CityId }).ToList();
            var articleList = _context.Articles.Select(e => e).ToList();
            return View(articleList);
        }

        public IActionResult Details(int id)
        {
            var article = _context.Articles
                .Include(a => a.Tro)
                .Include(a => a.ChungCu)
                .Include(a => a.House)
                .Include(a => a.Office)
                .Include(a => a.ImagesArticles)
                .Include(a => a.Account)
                .Include(a => a.CityArNavigation)
                .Include(a => a.DistrictArNavigation)
                .Include(a => a.Comments)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        public IActionResult Filters(string type, int city, int district, decimal? min, decimal? max)
        {
            var result = _context.Articles
                .Include(a => a.ImagesArticles).AsQueryable();

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
