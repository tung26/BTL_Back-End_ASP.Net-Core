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

        [HttpGet]
        public IActionResult Login()
        {
            if (IsLogin)
            {
                return RedirectToAction("Index", "Account", new { area = "Administrator", id = (int)this.IdUser });
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
                    Console.WriteLine(user.ImageUser);
                    if(user.ImageUser != null)
                    {
                        HttpContext.Session.SetString("ImageUser", user.ImageUser);
                        Console.WriteLine("ok");
                    }
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
    }
}
