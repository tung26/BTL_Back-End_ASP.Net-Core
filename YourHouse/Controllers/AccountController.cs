using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Models;
using YourHouse.Models.Entities;

namespace YourHouse.Controllers
{

    public class AccountController : Controller
    {
        private readonly YourHouseContext _context;
        private int IdUser { get; set; }
        private int role { get; set; }


        public AccountController(YourHouseContext context)
        {
            _context = context;
        }

        public bool isLogin()
        {
            var id = HttpContext.Session.GetInt32("id");

            if (!id.HasValue)
            {
                return false;
            }

            var user = _context.Accounts.FirstOrDefault(u => id.Value == u.AccountId);

            if (user == null)
            {
                return false;
            }

            this.IdUser = user.AccountId;
            this.role = user.RoleId;
            return true;
        }

        public IActionResult Index()
        {
            if (!isLogin())
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (isLogin())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginAccount acc)
        {
            if (ModelState.IsValid)
            {
                bool IsAvbacc = _context.Accounts.Any(b => b.Email == acc.Email && b.PasswordHash == acc.Password);
                if(IsAvbacc)
                {
                    var user = _context.Accounts.Where(b => b.Email == acc.Email && b.PasswordHash == acc.Password).FirstOrDefault();
                    HttpContext.Session.SetInt32("id", user.AccountId);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "Sai email hoặc mật khẩu");
                    ModelState.AddModelError("Password", "Sai email hoặc mật khẩu");
                }
            }
            return View(acc);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterAccount nAcc)
        {
            nAcc.RoleId = 3;
            ModelState.Remove("Role");
            ModelState.Remove("ImageUser");
            foreach (var error in ModelState)
            {
                foreach (var subError in error.Value.Errors)
                {
                    Console.WriteLine($"Lỗi tại {error.Key}: {subError.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                bool emailExists = _context.Accounts.Any(a => a.Email == nAcc.Email);
                bool phoneExists = _context.Accounts.Any(a => a.Phone == nAcc.Phone);
                bool success = true;
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng");
                    success = false;
                }

                if (phoneExists)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại đã được sử dụng");
                    success = false;
                }
                if (!success)
                {
                    return View(nAcc);
                }
                Account account = new Account()
                {
                    FullName = nAcc.FullName,
                    Email = nAcc.Email,
                    Phone = nAcc.Phone,
                    PasswordHash = nAcc.PasswordHash,
                    RoleId = nAcc.RoleId,
                };
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(nAcc);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("id");
            return RedirectToAction("Index", "Home");
        }
    }
}
