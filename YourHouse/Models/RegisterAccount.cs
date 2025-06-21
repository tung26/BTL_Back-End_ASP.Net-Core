using System.ComponentModel.DataAnnotations;
using YourHouse.Application.DTOs;


namespace YourHouse.Web.Models
{
    public class RegisterAccount : AccountDto
    {
        [Required(ErrorMessage = "yêu cầu nhập lại password")]
        [Compare("PasswordHash", ErrorMessage = "Nhập lại không đúng")]
        public string ConfirmPassword { get; set; }
    }
}
