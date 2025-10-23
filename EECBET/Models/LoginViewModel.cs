using System.ComponentModel.DataAnnotations;

namespace EECBET.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name = "帳號")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入驗證碼")]
        [Display(Name = "驗證碼")]
        public string Captcha { get; set; } = string.Empty;
    }
}
