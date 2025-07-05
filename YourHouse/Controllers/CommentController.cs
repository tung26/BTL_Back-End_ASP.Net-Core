using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;

namespace YourHouse.Web.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CommentDto commentDto)
        {
            if(!IsLogin)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                int id = await _commentService.AddCommentAsync(commentDto);
                var comment = await _commentService.GetCommentByIdAsync(id);
                return Json(new { success = true, idComment = id, createAt = comment.CreateAt });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if(comment.AccountId == this.IdUser || this.Role == 1)
                {
                    await _commentService.DeleteCommentAsync(id);
                    comment = await _commentService.GetCommentByIdAsync(id);
                    if (comment != null && comment.IsDelete == true)
                    {
                        return Json(new { success = true, isvalid = true });
                    }
                    return Json(new { success = true, isvalid = false });

                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
    }
}
