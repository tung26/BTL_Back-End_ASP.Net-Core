using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YourHouse.Web.Infrastructure.Data;


namespace YourHouse.Web.Controllers
{
    public class BaseController : Controller
    {
        protected int? IdUser { get; set; }
        protected bool IsLogin { get; set; } = false;
        protected int Role { get; set; }
        protected string FullName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = HttpContext.Session.GetInt32("id");
            var role = HttpContext.Session.GetInt32("role");
            var fullName = HttpContext.Session.GetString("fullName");

            if (id.HasValue)
            {
                this.IdUser = (int)id;
                this.Role = (int)role;
                this.IsLogin = true;
                this.FullName = fullName;

                ViewBag.IsLogin = this.IsLogin;
                ViewBag.UserName = this.FullName;
                ViewBag.IdUser = this.IdUser;
                ViewBag.RoleId = this.Role;
            }
            else
            {
                ViewBag.IsLogin = IsLogin;
            }

            base.OnActionExecuting(context);
        }
    }
}