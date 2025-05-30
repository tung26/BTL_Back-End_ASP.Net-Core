using Microsoft.AspNetCore.Mvc;

namespace YourHouse.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
