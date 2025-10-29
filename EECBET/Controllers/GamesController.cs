
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
            var games = await _context.GameList
                .Where(g => g.IsActive) // ✅ 若有此欄位，可保留，只顯示上架遊戲
                .OrderByDescending(g => g.ReleaseDate) // ✅ 最新遊戲在最前
                .ToListAsync();

            return View(games);
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


        //錯誤處理
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
