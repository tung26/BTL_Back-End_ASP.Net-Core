using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YourHouse.Models.Entities;

namespace YourHouse.Controllers
{
    public class BaseController : Controller
    {
        protected readonly YourHouseContext _context;
        protected int IdUser { get; set; }
        protected bool IsLogin { get; set; } = false;
        protected int Role { get; set; }

        public BaseController(YourHouseContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = HttpContext.Session.GetInt32("id");

            if (id.HasValue)
            {
                var user = _context.Accounts.FirstOrDefault(u => id.Value == u.AccountId);

                if (user == null)
                {
                    ViewBag.IsLogin = false;
                }
                else
                {
                    this.IdUser = user.AccountId;
                    this.Role = user.RoleId;
                    this.IsLogin = true;

                    ViewBag.IsLogin = IsLogin;
                    ViewBag.UserName = user.FullName;
                }
            }
            else
            {
                ViewBag.IsLogin = IsLogin;
            }

            base.OnActionExecuting(context);
        }
    }
}