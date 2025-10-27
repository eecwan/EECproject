using Microsoft.AspNetCore.Mvc;
using EECBET.Models;
using EECBET.Services;
using System.Linq;

namespace EECBET.Controllers
{
    public class BetController : Controller
    {
        private readonly DrawService _drawService;

        public BetController(DrawService drawService)
        {
            _drawService = drawService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Bet/SubmitBet")]
        public IActionResult SubmitBet([FromBody] BetRequest bet)
        {
            // 進行開獎
            var result = _drawService.Evaluate(bet);
            
            // 只返回開獎結果
            return Json(new
            {
                success = true,
                issueNo = result.IssueNo,
                drawNumbers = result.DrawNumbers,
                drawTime = result.DrawTime.ToString("yyyy-MM-dd HH:mm:ss"),
                wins = result.Wins,
                totalPayout = result.TotalPayout,
                betAmount = 0,
                newBalance = 0
            });
        }

        [HttpGet("Bet/History")]
        public IActionResult History(int count = 40)
        {
            var history = _drawService.GetRecentDraws(count)
                .Select(d => new
                {
                    issueNo = d.IssueNo,
                    drawNumbers = d.DrawNumbers,
                    drawTime = d.DrawTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    wins = d.Wins,
                    totalPayout = d.TotalPayout
                }).ToList();

            return Json(history);
        }
    }
}
