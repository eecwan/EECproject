
using EECBET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EECBET.Data;
using System.Diagnostics; //追蹤 Request 的類別

namespace EECBET.Controllers
{
    public class GamesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<GamesController> _logger;

        // 把 ApplicationDbContext 與 Logger 合併起來, ILogger<GamesController> 只是一個紀錄用
        public GamesController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _logger = logger;
            _context = context;

        }

        //    SlotGame 分頁    

        public async Task<IActionResult> SlotGame_home()
        {
            var allGames = await _context.GameList
            .Where(g => g.IsActive)
            .OrderByDescending(g => g.ReleaseDate)
            .ToListAsync();

            // 取高報酬前6個
            var highBonusGames = await _context.GameList
                .Where(g => g.GameCategory == "高報酬")
                .Take(6)
                .ToListAsync();

            var Exclusive = await _context.GameList
            .Where(g => g.GameCategory == "獨創")
            .Take(6)
            .ToListAsync();

            var classic = await _context.GameList
            .Where(g => g.GameCategory == "經典")
            .Take(6)
            .ToListAsync();

            var Megaways = await _context.GameList
            .Where(g => g.GameCategory == "頂級")
            .Take(6)
            .ToListAsync();

            // 用 其他上面的邏輯 用ViewBag 傳過去
            ViewBag.HighBonusGames = highBonusGames;
            ViewBag.Exclusive = Exclusive;
            ViewBag.classic = classic;
            ViewBag.Megaways = Megaways;
            //主模型
            return View(allGames);
        }

        public async Task<IActionResult> SlotGame_Exclusive()
        {
            var games = await _context.GameList
            .Where(g => g.GameCategory == "獨創")
            //把結果轉成 List
            .ToListAsync();
            return View(games);   //把資料傳給 SlotGame_Exclusive.cshtml
        }
        public async Task<IActionResult> SlotGame_HighBonus()
        {
            var games = await _context.GameList
            .Where(g => g.GameCategory == "高報酬")
            .ToListAsync();
            return View(games);
        }
        public async Task<IActionResult> SlotGame_classic()
        {
            var games = await _context.GameList
            .Where(g => g.GameCategory == "經典")
            .ToArrayAsync();
            return View(games);
        }
        public async Task<IActionResult> SlotGame_Megaways()
        {
            var games = await _context.GameList
            .Where(g => g.GameCategory == "頂級")
            .ToArrayAsync();
            return View(games);
        }

        public async Task<IActionResult> SlotGame_game(int id)
        {
            //後端檢查是否登入
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Member"); //導向MemberController內的Login()
            }

            //找出遊戲ID教出對應的網頁
            var games = await _context.GameList
            .FirstOrDefaultAsync(g => g.GameID == id);
            if (games == null)
            {
                return NotFound();
            }

            //記錄「進入遊戲」
            try
            {
                var record = new BetRecord
                {
                    MemberId = memberId.Value,
                    GameType = games.GameNameTW ?? games.GameNameEN ?? "未知遊戲",
                    IssueNo = 0,
                    BetAmount = 0,
                    WinAmount = 0,
                    Result = "進入遊戲",
                    PointsBefore = 0,
                    PointsAfter = 0,
                    CreatedAt = DateTime.UtcNow 
                };

                _context.BetRecords.Add(record);
                await _context.SaveChangesAsync(); //非同步呼叫

                _logger.LogInformation($"✅ 成功記錄遊戲：MemberId={memberId}, GameType={record.GameType}");
            
            }
            catch (Exception ex)
            {
                // 把內部例外也印出來
                var inner = ex.InnerException?.Message ?? "無內部例外";
                _logger.LogError(ex, $"❌ 儲存發生錯誤：{inner}");
               
            }

            return View(games);

        }

        //錯誤處理
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
