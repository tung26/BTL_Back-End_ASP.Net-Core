using System.ComponentModel.DataAnnotations;
using YourHouse.Web.Models.Entities;

namespace YourHouse.Web.Models
{
    public class RegisterAccount : Account
    {
        [Required(ErrorMessage = "yêu cầu nhập lại password")]
        [Compare("PasswordHash", ErrorMessage = "Nhập lại không đúng")]
        public string ConfirmPassword { get; set; }
    }
}
