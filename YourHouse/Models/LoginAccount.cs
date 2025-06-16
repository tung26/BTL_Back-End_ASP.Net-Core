using System.ComponentModel.DataAnnotations;

namespace YourHouse.Web.Models
{
    public class LoginAccount
    {
        [Required(ErrorMessage = "yêu cầu nhập email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "yêu cầu nhập password")]
        public string Password { get; set; }
    }
}
