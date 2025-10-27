using System;
using System.ComponentModel.DataAnnotations;

namespace EECBET.Models
{
  public class RegisterViewModel
  {
    [Required(ErrorMessage = "請輸入用戶名")]
    [StringLength(50, ErrorMessage = "用戶名長度不能超過50個字元")]
    [Display(Name = "用戶名")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入密碼")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "密碼長度必須在4-100個字元之間")]
    [DataType(DataType.Password)]
    [Display(Name = "密碼")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "請確認密碼")]
    [Compare("Password", ErrorMessage = "密碼與確認密碼不一致")]
    [DataType(DataType.Password)]
    [Display(Name = "確認密碼")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入電子郵件")]
    [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
    [StringLength(100)]
    [Display(Name = "電子郵件")]
    public string Email { get; set; } = string.Empty;

    public string? Lastname { get; set; }
    public string? Firstname { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Country { get; set; }
  }
}

