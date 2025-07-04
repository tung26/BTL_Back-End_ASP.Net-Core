using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourHouse.Infrastructure.Data;
using YourHouse.Application.Interfaces;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Infrastructure;
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

            ModelState.Remove("Role");
            ModelState.Remove("PasswordHash");
            if (ModelState.IsValid)
            {
                accChange.AccountId = acc.AccountId;
                accChange.FullName = acc.FullName;
                accChange.Phone = acc.Phone;
                accChange.Email = acc.Email;
                accChange.ImageUser = acc.ImageUser;
                accChange.Facebook = acc.Facebook;
                await _accountService.UpdateAccount(accChange);
                return RedirectToAction("Index", new {id = (int)IdUser});
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
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

        public async Task<IActionResult> Delete(int id)
        {
            if((IsLogin && id == this.IdUser) || this.Role == 1)
            {
                await _accountService.DeleteAccountAsync(id);
                
                if(id == this.IdUser)
                {
                    HttpContext.Session.Remove("id");
                    return Json(new { redirectUrl = Url.Action("Index", "Home", new { area = "" }) });
                } else
                {
                    return Json(new { redirectUrl = Url.Action("Manager", "Account", new { area = "Administrator" }) });
                }
            }

            return Json(new { success = true, redirectUrl = Url.Action("Login", "Account", new { area = "" }) });
        }

        public async Task<IActionResult> Manager()
        {
            if(IsLogin && this.Role == 1)
            {
                var accounts = await _accountService.GetAllAccountAsync();
                accounts = accounts.Where(x => x.RoleId != 1);

                return View(accounts.ToList());
            }

            if (IsLogin)
            {
                return RedirectToAction("Index", "MyArticle", new { area = "Administrator" });
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }
    }
}
