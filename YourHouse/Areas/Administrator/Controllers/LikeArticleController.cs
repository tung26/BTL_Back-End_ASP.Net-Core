using Microsoft.AspNetCore.Mvc;
using YourHouse.Application.Interfaces;

namespace YourHouse.Web.Areas.Administrator.Controllers
{
    public class LikeArticleController : Controller
    {
        private readonly ILikeArticleService _likeArticleService;
        private readonly IArticleService _articleService;
        
        public LikeArticleController(ILikeArticleService likeArticleService, IArticleService articleService)
        {
            _likeArticleService = likeArticleService;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
