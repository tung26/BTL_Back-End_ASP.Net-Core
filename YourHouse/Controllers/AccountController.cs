using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Web.Infrastructure.Data;
using YourHouse.Web.Models;
using YourHouse.Application.Interfaces;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Web.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var user = await _accountService.GetAccountByIdAsync(id);

            if (user == null)
            {
                if (this.IdUser != null)
                {
                    user = await _accountService.GetAccountByIdAsync((int)this.IdUser);
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //if (id != this.IdUser || this.IdUser == 0)
            //{
            //    if(this.IdUser != 0)
            //    {
            //        return RedirectToAction("Index");
            //    }
            //    return RedirectToAction("Login");
            //}

            var user = await _accountService.GetAccountByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] AccountDto acc)
        {
            var accChange = await _accountService.GetAccountByIdAsync(acc.AccountId);
            var accounts = await _accountService.GetAllAccountAsync();

            foreach(var account in accounts)
            {
                if (account.AccountId == accChange.AccountId) continue;
                if(account.Email == acc.Email)
                {
                    ModelState.AddModelError("Email", "Email đã được đăng kí ở tài khoản khác.");
                }
                if(account.Phone == acc.Phone)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại đã được đăng kí ở tài khoản khác.");
                }
            }

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
                accChange.AccountId = acc.AccountId;
                accChange.FullName = acc.FullName;
                accChange.Email = acc.Email;
                accChange.ImageUser = acc.ImageUser;
                await _accountService.UpdateAccount(accChange);

                return RedirectToAction("Index", new {id = this.IdUser});
            }
            
            return View(acc);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            //foreach (var error in ModelState)
            //{
            //    foreach (var subError in error.Value.Errors)
            //    {
            //        Console.WriteLine($"Lỗi tại {error.Key}: {subError.ErrorMessage}");
            //    }
            //}
            var user = await _accountService.GetAccountByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Login");
            }
            else if (id != this.IdUser)
            {
                return RedirectToAction("Index", new { id = this.IdUser });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePassword cp)
        {
            if (this.IdUser == null)
            {
                return RedirectToAction("Login");
            }

            var acc = await _accountService.GetAccountByIdAsync(cp.Id);
            if (ModelState.IsValid && acc != null)
            {
                if (cp.PastPass == cp.NewPass)
                {
                    ModelState.AddModelError("NewPass", "Yêu cầu nhập mật khẩu khác");
                }
                else if (cp.PastPass == acc.PasswordHash)
                {
                    acc.PasswordHash = cp.NewPass;
                    await _accountService.UpdateAccount(acc);
                    return RedirectToAction("Index", new {id = this.IdUser});
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
        public async Task<IActionResult> Login([FromForm] LoginAccount acc)
        {
            if (ModelState.IsValid)
            {
                bool IsAvbacc = await _accountService.IsValidAccount(email: acc.Email, password: acc.Password);
                if (IsAvbacc)
                {
                    var user = await _accountService.GetAccountByEmailAsync(acc.Email);
                    HttpContext.Session.SetInt32("id", user.AccountId);
                    HttpContext.Session.SetInt32("role", user.RoleId);
                    HttpContext.Session.SetString("fullName", user.FullName);
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
        public async Task<IActionResult> Register([FromForm] RegisterAccount nAcc)
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
                var accounts = await _accountService.GetAllAccountAsync();
                bool emailExists = accounts.Any(accounts => accounts.Email == nAcc.Email);
                bool phoneExists = accounts.Any(accounts=> accounts.Phone == nAcc.Phone);
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
                AccountDto account = new AccountDto()
                {
                    FullName = nAcc.FullName,
                    Email = nAcc.Email,
                    Phone = nAcc.Phone,
                    PasswordHash = nAcc.PasswordHash,
                    RoleId = nAcc.RoleId,
                };

                await _accountService.AddAccountAsync(account);
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
