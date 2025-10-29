using System.Diagnostics;
using EECBET.Models;
using Microsoft.AspNetCore.Mvc;

namespace EECBET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // public IActionResult SlotGame_home()
        // {
        //     return View();
        // }

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

        public IActionResult Privacy()
        {
            return View();
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
