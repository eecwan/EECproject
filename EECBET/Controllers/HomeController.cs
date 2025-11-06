using System.Diagnostics;
using EECBET.Models;
using Microsoft.AspNetCore.Mvc;
using EECBET.Data; //資料庫
using Microsoft.EntityFrameworkCore; //給ToListAsync


namespace EECBET.Controllers
{
    public class HomeController : Controller
    {
        //import 資料庫
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;// 資料庫內容
        }

        public async Task<IActionResult> Index()
        {
            var allGames = await _context.GameList
            .Where(g => g.IsActive)
            .OrderByDescending(g => g.ReleaseDate)
            .Take(8)
            .ToListAsync();
            return View(allGames);
        }

        public IActionResult Lottery()
        {
            return View();
        }
        public IActionResult Member()
        {
            return View();
        }

        public IActionResult LotteryMenu()
        {
            return View();
        }

        public IActionResult Lottery539()
        {
            return View();
        }

        public IActionResult LotteryLotto()
        {
            return View();
        }

        public IActionResult LotteryPower()
        {
            return View();
        }

        public IActionResult LotteryToday539()
        {
            return View();
        }

        public IActionResult LotteryDoubleColor()
        {
            return View();
        }

        public IActionResult LotteryScratch()
        {
            return View();
        }

        public IActionResult Points()
        {
            return RedirectToAction("Points", "Member");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 🔥 添加這個
        public IActionResult SlotMachine()
        {
            return View();
        }
    }
}
