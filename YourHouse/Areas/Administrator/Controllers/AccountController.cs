using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Web.Infrastructure.Data;
using YourHouse.Application.Interfaces;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Web.Infrastructure;
using YourHouse.Web.Controllers;
using YourHouse.Web.Areas.Administrator.Models;

namespace YourHouse.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
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
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != IdUser || IdUser == null)
            {
                if (IdUser == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                return RedirectToAction("Index", new { id = (int)IdUser });
            }

            var user = await _accountService.GetAccountByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
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
                accChange.Phone = acc.Phone;
                accChange.Email = acc.Email;
                accChange.ImageUser = acc.ImageUser;
                await _accountService.UpdateAccount(accChange);

                return RedirectToAction("Index", new {id = (int)IdUser});
            }
            
            return RedirectToAction("Index");
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
            if (id != IdUser || IdUser == null)
            {
                if (IdUser == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                return RedirectToAction("Index", new { id = (int)IdUser });
            }

            var user = await _accountService.GetAccountByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            else if (id != IdUser)
            {
                return RedirectToAction("Index", new { id = (int)IdUser });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePassword cp)
        {
            if (IdUser == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
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
                    return RedirectToAction("Index", new {id = (int)IdUser });
                }

                ModelState.AddModelError("PastPass", "Nhập sai mật khẩu cũ");
            }
            return View(cp);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("id");
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
