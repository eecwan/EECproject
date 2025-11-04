using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EECBET.Data;
using EECBET.Models;

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

                // 驗證密碼（簡單比對 - 學生作業用）
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
                member.LastLogin = DateOnly.FromDateTime(DateTime.UtcNow); // ✅ 修改
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

        // GET: /Member/Register01
        public IActionResult Register01()
        {
            return View();
        }

        // POST: /Member/Register01 (註冊表單提交)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register01(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 檢查用戶名是否已存在
                if (await _context.Members.AnyAsync(m => m.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "此用戶名已被使用");
                    return View(model);
                }

                // 檢查 Email 是否已存在
                if (await _context.Members.AnyAsync(m => m.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "此電子郵件已被註冊");
                    return View(model);
                }

                // 建立新會員
                var member = new Member
                {
                    Username = model.Username,
                    Password = model.Password, // 明文密碼（學生作業用）
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Gender = model.Gender,
                    Birthday = model.Birthday.HasValue
                        ? DateOnly.FromDateTime(model.Birthday.Value)   // ✅ 修改
                        : null,
                    Country = model.Country,
                    Points = 1000, // 新會員贈送1000點
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow) // ✅ 修改
                };

                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                // 自動登入
                HttpContext.Session.SetInt32("MemberId", member.Id);
                HttpContext.Session.SetString("Username", member.Username);

                _logger.LogInformation($"新會員 {member.Username} 註冊成功");

                // 設定註冊成功訊息
                ViewBag.RegisterSuccess = true;

                // 組合顯示名稱
                var displayName = !string.IsNullOrEmpty(model.Lastname) || !string.IsNullOrEmpty(model.Firstname)
                    ? $"{model.Lastname}{model.Firstname}"
                    : member.Username;
                ViewBag.Username = displayName;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊過程發生錯誤");
                ModelState.AddModelError("", "註冊失敗，請稍後再試");
                return View(model);
            }
        }

        // GET: /Member/Points
        public async Task<IActionResult> Points()
        {
            // 檢查登入狀態
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login");
            }

            // 從資料庫載入會員資訊
            var member = await _context.Members.FindAsync(memberId.Value);
            if (member == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            _logger.LogInformation($"載入會員資料: Username={member.Username}, Points={member.Points}, TotalBet={member.TotalBet}, TotalWin={member.TotalWin}");
            _logger.LogInformation($"載入會員資料: Firstname={member.Firstname}, Lastname={member.Lastname}, Country={member.Country}");

            // 傳遞會員資訊到 View
            ViewBag.Member = member;
            ViewBag.Username = member.Username;

            // 指定 View 位置
            return View("~/Views/Home/Points.cshtml");
        }

        // POST: /Member/UpdateProfile - 更新個人資料
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            try
            {
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                var member = await _context.Members.FindAsync(memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員資料" });
                }

                // 更新資料（允許更新為空值）
                member.Firstname = model.Firstname;
                member.Lastname = model.Lastname;
                member.Email = model.Email;
                member.Gender = model.Gender;
                member.Country = model.Country;

                if (model.Birthday.HasValue)
                    member.Birthday = DateOnly.FromDateTime(model.Birthday.Value); // ✅ 修改
                else if (model.Birthday == null)
                    member.Birthday = null;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "資料更新成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新個人資料時發生錯誤");
                return Json(new { success = false, message = "更新失敗，請稍後再試" });
            }
        }

        // GET: /Member/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: /Member/GetGameRecords - 獲取遊戲記錄（包含拉霸機）
        [HttpGet]
        public async Task<IActionResult> GetGameRecords()
        {
            try
            {
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                // 創建結果列表
                var allRecords = new List<object>();

                // 獲取彩票遊戲記錄
                var betRecords = await _context.BetRecords
                    .Where(r => r.MemberId == memberId.Value)
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(20)
                    .ToListAsync();

                // 添加彩票記錄到結果列表
                foreach (var r in betRecords)
                {
                    allRecords.Add(new
                    {
                        id = r.Id,
                        gameType = r.GameType,
                        issueNo = r.IssueNo.ToString(),
                        betAmount = r.BetAmount,
                        winAmount = r.WinAmount,
                        result = r.Result,
                        pointsBefore = r.PointsBefore,
                        pointsAfter = r.PointsAfter,
                        createdAt = r.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }

                // 嘗試獲取拉霸機記錄（如果表格有 MemberId 欄位）
                try
                {
                    var slotRecords = await _context.SlotRecords
                        .Where(r => r.MemberId == memberId.Value)
                        .OrderByDescending(r => r.PlayTime)
                        .Take(20)
                        .ToListAsync();

                    // 添加拉霸機記錄到結果列表
                    foreach (var r in slotRecords)
                    {
                        allRecords.Add(new
                        {
                            id = r.Id,
                            gameType = "拉霸機",
                            issueNo = "-",
                            betAmount = (decimal)r.Bet,
                            winAmount = (decimal)r.Reward,
                            result = r.Result,
                            pointsBefore = (decimal)(r.CreditsAfter - r.Reward + r.Bet),
                            pointsAfter = (decimal)r.CreditsAfter,
                            createdAt = r.PlayTime.ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }
                }
                catch (Exception slotEx)
                {
                    // 如果 SlotRecords 表格還沒有 MemberId 欄位，忽略錯誤
                    _logger.LogWarning(slotEx, "無法查詢拉霸機記錄，可能是資料庫尚未更新 MemberId 欄位");
                }

                // 按時間排序並取最近 30 筆
                var sortedRecords = allRecords
                    .OrderByDescending(r => ((dynamic)r).createdAt)
                    .Take(30)
                    .ToList();

                return Json(new { success = true, records = sortedRecords });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取遊戲記錄時發生錯誤");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Member/GetBalance - 獲取當前餘額（包含拉霸機）
        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                var member = await _context.Members.FindAsync(memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員" });
                }

                // 計算今日盈虧（今天的總中獎額 - 今天的總投注額）
                var todayStart = DateTime.UtcNow.Date;
                var todayEnd = todayStart.AddDays(1);

                // 彩票遊戲今日記錄
                var todayBetRecords = await _context.BetRecords
                    .Where(r => r.MemberId == memberId.Value &&
                                r.CreatedAt >= todayStart &&
                                r.CreatedAt < todayEnd)
                    .ToListAsync();

                // 拉霸機今日記錄（如果表格有 MemberId 欄位）
                decimal slotTodayBet = 0;
                decimal slotTodayWin = 0;
                try
                {
                    var todaySlotRecords = await _context.SlotRecords
                        .Where(r => r.MemberId == memberId.Value &&
                                    r.PlayTime >= todayStart &&
                                    r.PlayTime < todayEnd)
                        .ToListAsync();

                    slotTodayBet = todaySlotRecords.Sum(r => r.Bet);
                    slotTodayWin = todaySlotRecords.Sum(r => r.Reward);
                }
                catch (Exception slotEx)
                {
                    // 如果 SlotRecords 表格還沒有 MemberId 欄位，忽略錯誤
                    _logger.LogWarning(slotEx, "無法查詢拉霸機今日記錄");
                }

                var todayBet = todayBetRecords.Sum(r => r.BetAmount) + slotTodayBet;
                var todayWin = todayBetRecords.Sum(r => r.WinAmount) + slotTodayWin;
                var todayProfit = todayWin - todayBet;

                _logger.LogInformation($"獲取餘額: MemberId={memberId}, Points={member.Points}, TotalBet={member.TotalBet}, TotalWin={member.TotalWin}, TodayProfit={todayProfit}");

                return Json(new
                {
                    success = true,
                    balance = member.Points,
                    totalBet = member.TotalBet,
                    totalWin = member.TotalWin,
                    todayProfit = todayProfit
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取餘額時發生錯誤");
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        // GET: /Member/RecordGamePlay?gameId=123
[HttpGet]
public IActionResult RecordGamePlay(int id)
{
    var memberId = HttpContext.Session.GetInt32("MemberId");
    if (memberId == null)
    {
        return Json(new { success = false, message = "未登入" });
    }

    // 依據 GameID 找遊戲資料
    var game = _context.GameList.FirstOrDefault(g => g.GameID == id);
    if (game == null)
    {
        return Json(new { success = false, message = "找不到遊戲" });
    }

   try
{
    var record = new BetRecord
    {
        MemberId = memberId.Value,
        GameType = game.GameNameTW ?? game.GameNameEN ?? "未知遊戲",
        IssueNo = 0,
        BetAmount = 0,
        WinAmount = 0,
        Result = "進入遊戲",
        PointsBefore = 0,
        PointsAfter = 0,
        CreatedAt = DateTime.UtcNow // ✅ 改這裡！
    };

    _context.BetRecords.Add(record);
    _context.SaveChanges();

    _logger.LogInformation($"✅ 成功記錄遊戲：MemberId={memberId}, GameType={record.GameType}");
    return Json(new { success = true });
}
catch (Exception ex)
{
    // 🔍 把內部例外也印出來
    var inner = ex.InnerException?.Message ?? "無內部例外";
    _logger.LogError(ex, $"❌ 儲存發生錯誤：{inner}");
    return Json(new { success = false, message = inner });
}
}

        // 生成隨機驗證碼
        private string GenerateCaptcha()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // 排除容易混淆的字元
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // 密碼加密方法已移除 - 學生作業使用明文密碼
    }
}
