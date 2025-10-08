using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class BetController : Controller
    {
        private readonly DrawService _drawService;

        public BetController()
        {
            _drawService = new DrawService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // 對應 Views/Bet/Index.cshtml
        }

        [HttpPost("Bet/SubmitBet")]
        public IActionResult SubmitBet([FromBody] BetRequest bet)
        {
            if (bet == null || bet.Numbers.Count < 2)
                return BadRequest(new { error = "至少選 2 個號碼" });

            var result = _drawService.Evaluate(bet);
            return Json(result);
        }
    }
}
