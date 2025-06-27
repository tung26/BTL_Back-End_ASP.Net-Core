using System.ComponentModel.DataAnnotations;

namespace YourHouse.Web.Areas.Administrator.Models
{
    public class ChangePassword
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu.")]
        public string PastPass { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu.")]
        [MinLength(9, ErrorMessage = "Mật khẩu phải có ít nhất 9 kí tự.")]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?""{}|<>]).+$", ErrorMessage = "Mật khẩu phải có ít nhất một ký tự đặc biệt.")]
        public string NewPass { get; set; }
        [Required(ErrorMessage = "yêu cầu nhập lại password")]
        [Compare("NewPass", ErrorMessage = "Nhập lại không đúng")]
        public string ConfirmNewPassword { get; set; }
    }
}
