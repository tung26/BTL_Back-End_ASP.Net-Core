using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Application.Interfaces;
using YourHouse.Infrastructure;
using YourHouse.Web.Models;


namespace YourHouse.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ILikeArticleService _likeArticleService;
        private readonly IImageArticleService _imageArticleService;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, ICommentService commentService, ILikeArticleService likeArticleService, IImageArticleService imageArticleService)
        {
            _logger = logger;
            _articleService = articleService;
            _commentService = commentService;
            _likeArticleService = likeArticleService;
            _imageArticleService = imageArticleService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticleAsync();
            var comments = await _commentService.GetAllCommentAsync();
            var likeArticles = await _likeArticleService.GetAllLikeArticleAsync();
            var images = await _imageArticleService.GetAllImageArticleAsync();

            var result = articles.Select(article => new {
                Article = article,
                CommentCount = comments.Count(c => c.ArticleId == article.ArticleId),
                LikeCount = likeArticles.Count(l => l.ArticleId == article.ArticleId),
                TotalInteraction = comments.Count(c => c.ArticleId == article.ArticleId) + likeArticles.Count(l => l.ArticleId == article.ArticleId),
                Images = images.Where(i => i.ArticleId == article.ArticleId).ToList()
            });

            var articleCommentTops = result.Where(r => r.CommentCount > 0).OrderByDescending(c => c.CommentCount);
            var articleLikeTops = result.Where(r => r.LikeCount > 0).OrderByDescending(result => result.LikeCount);
            var articleoutstandings = result.Where(r => r.TotalInteraction > 0).OrderByDescending(c => c.Article.CreateAt).ThenByDescending(c => c.TotalInteraction);

            ViewBag.commentTops = articleCommentTops.ToList();
            ViewBag.likeTops = articleLikeTops.ToList();
            ViewBag.outstandings = articleoutstandings.ToList();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
