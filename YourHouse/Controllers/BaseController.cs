using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YourHouse.Web.Infrastructure.Data;


namespace YourHouse.Web.Controllers
{
    public class BaseController : Controller
    {
        protected int? IdUser { get; set; }
        protected bool IsLogin { get; set; } = false;
        protected int Role { get; set; } = 3;
        protected string FullName { get; set; }
        protected string? ImageUser { get; set; } = null;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetInt32("role");
            var fullName = HttpContext.Session.GetString("fullName");
            var ImageUser = HttpContext.Session.GetString("ImageUser");

            if (id.HasValue)
            {
                this.IdUser = (int)id;
                this.Role = (int)role;
                this.IsLogin = true;
                this.FullName = fullName;
                this.ImageUser = ImageUser;

                ViewBag.IsLogin = this.IsLogin;
                ViewBag.UserName = this.FullName;
                ViewBag.IdUser = (int)this.IdUser;
                ViewBag.RoleId = this.Role;
                ViewBag.ImageUser = this.ImageUser;
            }
            else
            {
                ViewBag.IsLogin = IsLogin;
            }

            base.OnActionExecuting(context);
        }
    }
}