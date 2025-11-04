
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

            // 用 ViewBag 傳過去（主模型保持原來的）
            ViewBag.HighBonusGames = highBonusGames;
            ViewBag.Exclusive = Exclusive;
            ViewBag.classic = classic;
            ViewBag.Megaways = Megaways;

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
            var games = await _context.GameList
          .FirstOrDefaultAsync(g => g.GameID == id);
          if(games==null)
              {
                return NotFound();
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
