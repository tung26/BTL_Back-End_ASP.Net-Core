using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourHouse.Application.DTOs
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập họ tên.")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu.")]
        [MinLength(9, ErrorMessage = "Mật khẩu phải có ít nhất 9 kí tự.")]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?""{}|<>]).+$", ErrorMessage = "Mật khẩu phải có ít nhất một ký tự đặc biệt.")]
        public string PasswordHash { get; set; } = null!;
        [Required(ErrorMessage = "Yêu cầu nhập email.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại.")]
        [RegularExpression(@"^(?! )[^\s]+(?:[-. ]?[0-9]+)*$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string Phone { get; set; } = null!;

        public int RoleId { get; set; }
        public string ImageUser { get; set; } = null!;
    }
}
