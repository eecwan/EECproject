using System.Diagnostics;
using EECBET.Models;
using Microsoft.AspNetCore.Mvc;

namespace EECBET.Controllers
{
    public class GamesController : Controller
    {
        private readonly ILogger<GamesController> _logger;

        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
        }
       
        public IActionResult SlotGame_Exclusive()
        {
            return View();
        }
        public IActionResult SlotGame_HighBonus()
        {
            return View();
        }
        public IActionResult SlotGame_classic()
        {
            return View();
        }
        public IActionResult SlotGame_Megaways()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
