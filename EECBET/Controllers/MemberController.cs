using Microsoft.AspNetCore.Mvc;

namespace EECBET.Controllers
{
    public class MemberController : Controller
    {
        // GET: /Member/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: /Member/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Member/Login (登入表單提交)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // 這裡處理登入邏輯
            // 暫時先返回 View
            return View();
        }

        // POST: /Member/Register (註冊表單提交)
        [HttpPost]
        public IActionResult Register(string username, string password, string email)
        {
            // 這裡處理註冊邏輯
            return View();
        }
    }
}