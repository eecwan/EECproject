using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EECBET.Data;
using EECBET.Models;
using System.Security.Cryptography;
using System.Text;

namespace EECBET.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MemberController> _logger;

        public MemberController(ApplicationDbContext context, ILogger<MemberController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Member/Login
        public IActionResult Login()
        {
            // 生成驗證碼並存入 Session
            var captcha = GenerateCaptcha();
            HttpContext.Session.SetString("Captcha", captcha);
            ViewBag.Captcha = captcha;
            
            return View();
        }

        // POST: /Member/Login (登入表單提交)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // 重新生成驗證碼
                var newCaptcha = GenerateCaptcha();
                HttpContext.Session.SetString("Captcha", newCaptcha);
                ViewBag.Captcha = newCaptcha;
                return View(model);
            }

            // 驗證驗證碼
            var sessionCaptcha = HttpContext.Session.GetString("Captcha");
            if (string.IsNullOrEmpty(sessionCaptcha) || 
                !model.Captcha.Equals(sessionCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Captcha", "驗證碼錯誤");
                
                // 重新生成驗證碼
                var newCaptcha = GenerateCaptcha();
                HttpContext.Session.SetString("Captcha", newCaptcha);
                ViewBag.Captcha = newCaptcha;
                
                return View(model);
            }

            try
            {
                // 查詢使用者
                var member = await _context.Members
                    .FirstOrDefaultAsync(m => m.Username == model.Username);

                if (member == null)
                {
                    ModelState.AddModelError("", "帳號或密碼錯誤");
                    
                    // 重新生成驗證碼
                    var newCaptcha = GenerateCaptcha();
                    HttpContext.Session.SetString("Captcha", newCaptcha);
                    ViewBag.Captcha = newCaptcha;
                    
                    return View(model);
                }

                // 驗證密碼（這裡使用簡單比對，實際應用應使用加密）
                // 如果密碼有加密，請使用: VerifyPassword(model.Password, member.Password)
                if (member.Password != model.Password)
                {
                    ModelState.AddModelError("", "帳號或密碼錯誤");
                    
                    // 重新生成驗證碼
                    var newCaptcha = GenerateCaptcha();
                    HttpContext.Session.SetString("Captcha", newCaptcha);
                    ViewBag.Captcha = newCaptcha;
                    
                    return View(model);
                }

                // 登入成功，更新最後登入時間
                member.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();

                // 設定 Session
                HttpContext.Session.SetInt32("MemberId", member.Id);
                HttpContext.Session.SetString("Username", member.Username);

                _logger.LogInformation($"使用者 {member.Username} 登入成功");

                // 設定成功訊息並返回視圖（使用 JavaScript 延遲跳轉）
                ViewBag.LoginSuccess = true;
                ViewBag.Username = member.Username;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登入過程發生錯誤");
                ModelState.AddModelError("", "登入失敗，請稍後再試");
                
                // 重新生成驗證碼
                var newCaptcha = GenerateCaptcha();
                HttpContext.Session.SetString("Captcha", newCaptcha);
                ViewBag.Captcha = newCaptcha;
                
                return View(model);
            }
        }

        // GET: /Member/Register
        public IActionResult Register01()
        {
            return View();
        }

        // GET: /Member/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // 生成隨機驗證碼
        private string GenerateCaptcha()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // 排除容易混淆的字元
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // 密碼加密 (SHA256) - 如果需要的話
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // 驗證密碼
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var inputHash = HashPassword(inputPassword);
            return inputHash == storedHash;
        }
    }
}
