using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Models;
using YourHouse.Models.Entities;

namespace YourHouse.Controllers
{

    public class AccountController : BaseController
    {
        //private readonly YourHouseContext _context;
        //private int IdUser { get; set; }
        //private int Role { get; set; }


        public AccountController(YourHouseContext context) : base(context) { }

        //public bool IsLogin
        //{
        //    var id = HttpContext.Session.GetInt32("id");

        //    if (!id.HasValue)
        //    {
        //        ViewBag.IsLogin = false;
        //        return false;
        //    }

        //    var user = _context.Accounts.FirstOrDefault(u => id.Value == u.AccountId);

        //    if (user == null)
        //    {
        //        ViewBag.IsLogin = false;
        //        return false;
        //    }

        //    this.IdUser = user.AccountId;
        //    this.Role = user.RoleId;

        //    ViewBag.IsLogin = true;
        //    ViewBag.UserName = user.FullName;

        //    return true;
        //}
        public IActionResult Index(int id)
        {
            var user = _context.Accounts.FirstOrDefault(u => u.AccountId == id);

            if (user == null)
            {
                if(this.IdUser != 0)
                {
                    user = _context.Accounts.FirstOrDefault(u => u.AccountId == this.IdUser);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id != this.IdUser || this.IdUser == 0)
            {
                if(this.IdUser != 0)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Login");
            }

            var user = _context.Accounts.FirstOrDefault(u => u.AccountId == id);

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] Account acc)
        {
            var accChange = _context.Accounts.FirstOrDefault(a => a.AccountId == acc.AccountId);
            //acc.Phone = accChange.Phone;
            ModelState.Remove("Role");
            ModelState.Remove("Phone");
            ModelState.Remove("PasswordHash");
            //foreach (var error in ModelState)
            //{
            //    foreach (var subError in error.Value.Errors)
            //    {
            //        Console.WriteLine($"Lỗi tại {error.Key}: {subError.ErrorMessage}");
            //    }
            //}
            if (ModelState.IsValid)
            {
                accChange.FullName = acc.FullName;
                accChange.Email = acc.Email;
                accChange.ImageUser = acc.ImageUser;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(acc);
        }

        [HttpGet]
        public IActionResult ChangePassword(int? id)
        {
            //foreach (var error in ModelState)
            //{
            //    foreach (var subError in error.Value.Errors)
            //    {
            //        Console.WriteLine($"Lỗi tại {error.Key}: {subError.ErrorMessage}");
            //    }
            //}
            if (this.IdUser == 0 || this.IdUser != id)
            {
                return RedirectToAction("Login");
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword([FromForm] ChangePassword cp)
        {
            if (this.IdUser == 0)
            {
                return RedirectToAction("Login");
            }

            var acc = _context.Accounts.FirstOrDefault(a => a.AccountId == cp.Id);
            if (ModelState.IsValid && acc != null)
            {
                if (cp.PastPass == cp.NewPass)
                {
                    ModelState.AddModelError("NewPass", "Yêu cầu nhập mật khẩu khác");
                }
                else if (cp.PastPass == acc.PasswordHash)
                {
                    acc.PasswordHash = cp.NewPass;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("PastPass", "Nhập sai mật khẩu cũ");
            }
            return View(cp);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (IsLogin)
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
            if (IsLogin && Role != 1)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterAccount nAcc)
        {
            nAcc.RoleId = 3;
            ModelState.Remove("Role");
            ModelState.Remove("ImageUser");
            //foreach (var error in ModelState)
            //{
            //    foreach (var subError in error.Value.Errors)
            //    {
            //        Console.WriteLine($"Lỗi tại {error.Key}: {subError.ErrorMessage}");
            //    }
            //}
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
