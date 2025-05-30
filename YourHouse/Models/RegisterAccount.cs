using System.ComponentModel.DataAnnotations;
using YourHouse.Models.Entities;

namespace YourHouse.Models
{
    public class RegisterAccount : Account
    {
        [Required(ErrorMessage = "yêu cầu nhập lại password")]
        [Compare("PasswordHash", ErrorMessage = "Nhập lại không đúng")]
        public string ConfirmPassword { get; set; }
    }
}
