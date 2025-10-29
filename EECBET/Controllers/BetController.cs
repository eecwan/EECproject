using Microsoft.AspNetCore.Mvc;
using EECBET.Models;
using EECBET.Services;
using EECBET.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace EECBET.Controllers
{
    public class BetController : Controller
    {
        private readonly DrawService _drawService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BetController> _logger;

        public BetController(DrawService drawService, ApplicationDbContext context, ILogger<BetController> logger)
        {
            _drawService = drawService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Bet/SubmitBet")]
        public async Task<IActionResult> SubmitBet([FromBody] BetRequest bet)
        {
            try
            {
                // 檢查登入狀態
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                // 獲取會員（使用 AsNoTracking 關閉追蹤，然後重新附加）
                var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員" });
                }

                _logger.LogInformation($"投注請求: MemberId={memberId}, Points={member.Points}, TotalAmount={bet.TotalAmount}");

                // 檢查餘額
                if (member.Points < bet.TotalAmount)
                {
                    return Json(new { success = false, message = "餘額不足" });
                }

                // 進行開獎
                var result = _drawService.Evaluate(bet);

                // 計算盈虧
                var winAmount = result.TotalPayout;
                var pointsBefore = member.Points;

                // 更新會員點數
                var pointsAfter = member.Points - bet.TotalAmount + winAmount;
                var totalBetAfter = member.TotalBet + bet.TotalAmount;
                var totalWinAfter = member.TotalWin + winAmount;

                _logger.LogInformation($"點數更新: PointsBefore={pointsBefore}, BetAmount={bet.TotalAmount}, WinAmount={winAmount}, PointsAfter={pointsAfter}");

                // 使用 SQL 直接更新會員點數
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE members SET points = {0}, total_bet = {1}, total_win = {2} WHERE id = {3}",
                    pointsAfter,
                    totalBetAfter,
                    totalWinAfter,
                    memberId.Value
                );

                // 保存投注記錄
                var betRecord = new BetRecord
                {
                    MemberId = memberId.Value,
                    GameType = "539", // 根據不同遊戲類型設定
                    IssueNo = result.IssueNo,
                    BetNumbers = JsonSerializer.Serialize(bet.Numbers),
                    BetAmount = bet.TotalAmount,
                    WinningNumbers = JsonSerializer.Serialize(result.DrawNumbers),
                    WinAmount = winAmount,
                    PointsBefore = pointsBefore,
                    PointsAfter = pointsAfter,
                    Result = winAmount > 0 ? $"中獎 {winAmount} 元" : "未中獎",
                    CreatedAt = DateTime.UtcNow
                };

                _context.BetRecords.Add(betRecord);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"投注記錄已保存: GameType={betRecord.GameType}, BetAmount={betRecord.BetAmount}, WinAmount={betRecord.WinAmount}");

                // 返回開獎結果
                return Json(new
                {
                    success = true,
                    issueNo = result.IssueNo,
                    drawNumbers = result.DrawNumbers,
                    drawTime = result.DrawTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    wins = result.Wins,
                    totalPayout = result.TotalPayout,
                    betAmount = bet.TotalAmount,
                    newBalance = pointsAfter,
                    debug = new { pointsBefore, betAmount = bet.TotalAmount, winAmount, pointsAfter }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "投注處理錯誤");
                return Json(new { success = false, message = ex.Message, error = ex.ToString() });
            }
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

        // 統一的投注處理方法
        private async Task<IActionResult> ProcessBetAsync(string gameType, object betData, Func<object, DrawResult> evaluateBet, Func<object, string> serializeBetNumbers)
        {
            try
            {
                // 檢查登入狀態
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                // 獲取會員（使用 AsNoTracking）
                var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員" });
                }

                // 取得投注金額（使用反射或動態）
                decimal totalAmount = 0;
                if (betData is BetRequest bet)
                    totalAmount = bet.TotalAmount;
                else if (betData is LottoBetRequest lotto)
                    totalAmount = lotto.TotalAmount;
                else if (betData is PowerBetRequest power)
                    totalAmount = power.TotalAmount;
                else if (betData is DoubleColorBetRequest doubleColor)
                    totalAmount = doubleColor.TotalAmount;
                else if (betData is Today539BetRequest today539)
                    totalAmount = today539.TotalAmount;

                // 檢查餘額
                if (member.Points < totalAmount)
                {
                    return Json(new { success = false, message = "餘額不足" });
                }

                // 進行開獎
                var result = evaluateBet(betData);

                // 計算盈虧
                var winAmount = result.TotalPayout;
                var pointsBefore = member.Points;

                // 計算新點數
                var pointsAfter = member.Points - totalAmount + winAmount;
                var totalBetAfter = member.TotalBet + totalAmount;
                var totalWinAfter = member.TotalWin + winAmount;

                // 使用 SQL 直接更新會員點數
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE members SET points = {0}, total_bet = {1}, total_win = {2} WHERE id = {3}",
                    pointsAfter,
                    totalBetAfter,
                    totalWinAfter,
                    memberId.Value
                );

                // 保存投注記錄
                var betRecord = new BetRecord
                {
                    MemberId = memberId.Value,
                    GameType = gameType,
                    IssueNo = result.IssueNo,
                    BetNumbers = serializeBetNumbers(betData),
                    BetAmount = totalAmount,
                    WinningNumbers = JsonSerializer.Serialize(result.DrawNumbers),
                    WinAmount = winAmount,
                    PointsBefore = pointsBefore,
                    PointsAfter = pointsAfter,
                    Result = winAmount > 0 ? $"中獎 {winAmount} 元" : "未中獎",
                    CreatedAt = DateTime.UtcNow
                };

                _context.BetRecords.Add(betRecord);
                await _context.SaveChangesAsync();

                // 返回開獎結果
                return Json(new
                {
                    success = true,
                    issueNo = result.IssueNo,
                    drawNumbers = result.DrawNumbers,
                    drawTime = result.DrawTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    wins = result.Wins,
                    totalPayout = result.TotalPayout,
                    betAmount = totalAmount,
                    newBalance = pointsAfter
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // 大樂透投注
        [HttpPost("Bet/SubmitLottoBet")]
        public async Task<IActionResult> SubmitLottoBet([FromBody] LottoBetRequest bet)
        {
            return await ProcessBetAsync("大樂透", bet, betData =>
            {
                var lottoBet = betData as LottoBetRequest;
                var drawService = new DrawService(new DrawHistoryService());

                // 轉換為BetRequest格式（假設大樂透使用相同邏輯）
                var betRequest = new BetRequest
                {
                    Numbers = lottoBet?.Numbers ?? new List<int>(),
                    TotalAmount = lottoBet?.TotalAmount ?? 0
                };

                return drawService.Evaluate(betRequest);
            }, betData =>
            {
                var lottoBet = betData as LottoBetRequest;
                return JsonSerializer.Serialize(lottoBet?.Numbers ?? new List<int>());
            });
        }

        // 威力彩投注
        [HttpPost("Bet/SubmitPowerBet")]
        public async Task<IActionResult> SubmitPowerBet([FromBody] PowerBetRequest bet)
        {
            try
            {
                // 檢查登入狀態
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                // 獲取會員
                var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員" });
                }

                var totalAmount = bet?.TotalAmount ?? 0;
                _logger.LogInformation($"威力彩投注: MemberId={memberId}, TotalAmount={totalAmount}");

                // 檢查餘額
                if (member.Points < totalAmount)
                {
                    return Json(new { success = false, message = "餘額不足" });
                }

                // 生成開獎號碼（6個第一區 + 1個第二區）
                var firstAreaDraw = BetHelper.GenerateUniqueNumbers(6, 1, 38);
                var secondAreaDraw = BetHelper.GenerateUniqueNumbers(1, 1, 8);
                var firstAreaNumbers = firstAreaDraw.OrderBy(x => x).ToList();
                var secondAreaNumbers = secondAreaDraw.OrderBy(x => x).ToList();

                // 計算中獎金額（簡化版本）
                var winAmount = BetHelper.CalculatePowerWin(bet?.FirstAreaNumbers ?? new List<int>(),
                                                   bet?.SecondAreaNumbers ?? new List<int>(),
                                                   firstAreaNumbers, secondAreaNumbers, totalAmount);

                var pointsBefore = member.Points;
                var pointsAfter = member.Points - totalAmount + winAmount;
                var totalBetAfter = member.TotalBet + totalAmount;
                var totalWinAfter = member.TotalWin + winAmount;

                // 使用 SQL 直接更新會員點數
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE members SET points = {0}, total_bet = {1}, total_win = {2} WHERE id = {3}",
                    pointsAfter, totalBetAfter, totalWinAfter, memberId.Value
                );

                // 保存投注記錄
                var betRecord = new BetRecord
                {
                    MemberId = memberId.Value,
                    GameType = "威力彩",
                    IssueNo = DateTime.UtcNow.Ticks,
                    BetNumbers = JsonSerializer.Serialize(new { First = bet?.FirstAreaNumbers, Second = bet?.SecondAreaNumbers }),
                    BetAmount = totalAmount,
                    WinningNumbers = JsonSerializer.Serialize(new { First = firstAreaNumbers, Second = secondAreaNumbers }),
                    WinAmount = winAmount,
                    PointsBefore = pointsBefore,
                    PointsAfter = pointsAfter,
                    Result = winAmount > 0 ? $"中獎 {winAmount} 元" : "未中獎",
                    CreatedAt = DateTime.UtcNow
                };

                _context.BetRecords.Add(betRecord);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"威力彩記錄已保存: TotalAmount={totalAmount}, WinAmount={winAmount}, PointsAfter={pointsAfter}");

                return Json(new
                {
                    success = true,
                    issueNo = betRecord.IssueNo,
                    firstAreaNumbers = firstAreaNumbers,
                    secondAreaNumbers = secondAreaNumbers,
                    wins = winAmount > 0 ? new[] { new { type = "中獎", totalPayout = winAmount } } : new object[0],
                    totalPayout = winAmount,
                    betAmount = totalAmount,
                    newBalance = pointsAfter
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "威力彩投注錯誤");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // 雙色球投注
        [HttpPost("Bet/SubmitDoubleColorBet")]
        public async Task<IActionResult> SubmitDoubleColorBet([FromBody] DoubleColorBetRequest bet)
        {
            try
            {
                // 檢查登入狀態
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                // 獲取會員
                var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員" });
                }

                var totalAmount = bet?.TotalAmount ?? 0;
                _logger.LogInformation($"雙色球投注: MemberId={memberId}, TotalAmount={totalAmount}");

                // 檢查餘額
                if (member.Points < totalAmount)
                {
                    return Json(new { success = false, message = "餘額不足" });
                }

                // 生成開獎號碼（6個紅球 + 1個藍球）
                var redDraw = BetHelper.GenerateUniqueNumbers(6, 1, 33);
                var blueDraw = BetHelper.GenerateUniqueNumbers(1, 1, 16);
                var redNumbers = redDraw.OrderBy(x => x).ToList();
                var blueNumbers = blueDraw.OrderBy(x => x).ToList();

                // 計算中獎金額
                var winAmount = BetHelper.CalculateDoubleColorWin(bet?.RedNumbers ?? new List<int>(),
                                                                  bet?.BlueNumbers ?? new List<int>(),
                                                                  redNumbers, blueNumbers, totalAmount);

                var pointsBefore = member.Points;
                var pointsAfter = member.Points - totalAmount + winAmount;
                var totalBetAfter = member.TotalBet + totalAmount;
                var totalWinAfter = member.TotalWin + winAmount;

                // 使用 SQL 直接更新會員點數
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE members SET points = {0}, total_bet = {1}, total_win = {2} WHERE id = {3}",
                    pointsAfter, totalBetAfter, totalWinAfter, memberId.Value
                );

                // 保存投注記錄
                var betRecord = new BetRecord
                {
                    MemberId = memberId.Value,
                    GameType = "雙色球",
                    IssueNo = DateTime.UtcNow.Ticks,
                    BetNumbers = JsonSerializer.Serialize(new { Red = bet?.RedNumbers, Blue = bet?.BlueNumbers }),
                    BetAmount = totalAmount,
                    WinningNumbers = JsonSerializer.Serialize(new { Red = redNumbers, Blue = blueNumbers }),
                    WinAmount = winAmount,
                    PointsBefore = pointsBefore,
                    PointsAfter = pointsAfter,
                    Result = winAmount > 0 ? $"中獎 {winAmount} 元" : "未中獎",
                    CreatedAt = DateTime.UtcNow
                };

                _context.BetRecords.Add(betRecord);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"雙色球記錄已保存: TotalAmount={totalAmount}, WinAmount={winAmount}, PointsAfter={pointsAfter}");

                return Json(new
                {
                    success = true,
                    issueNo = betRecord.IssueNo,
                    redNumbers = redNumbers,
                    blueNumbers = blueNumbers,
                    wins = winAmount > 0 ? new[] { new { type = "中獎", totalPayout = winAmount } } : new object[0],
                    totalPayout = winAmount,
                    betAmount = totalAmount,
                    newBalance = pointsAfter
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "雙色球投注錯誤");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // 今彩539投注
        [HttpPost("Bet/SubmitToday539Bet")]
        public async Task<IActionResult> SubmitToday539Bet([FromBody] Today539BetRequest bet)
        {
            return await ProcessBetAsync("今彩539", bet, betData =>
            {
                var todayBet = betData as Today539BetRequest;
                var drawService = new DrawService(new DrawHistoryService());

                var betRequest = new BetRequest
                {
                    Numbers = todayBet?.Numbers ?? new List<int>(),
                    TotalAmount = todayBet?.TotalAmount ?? 0
                };

                return drawService.Evaluate(betRequest);
            }, betData =>
            {
                var todayBet = betData as Today539BetRequest;
                return JsonSerializer.Serialize(todayBet?.Numbers ?? new List<int>());
            });
        }

        // 刮刮樂購買
        [HttpPost("Bet/PurchaseScratchCards")]
        public async Task<IActionResult> PurchaseScratchCards([FromBody] ScratchPurchaseRequest request)
        {
            try
            {
                // 檢查登入狀態
                var memberId = HttpContext.Session.GetInt32("MemberId");
                if (memberId == null)
                {
                    return Json(new { success = false, message = "未登入" });
                }

                // 獲取會員
                var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId.Value);
                if (member == null)
                {
                    return Json(new { success = false, message = "找不到會員" });
                }

                var totalCost = request.CardValue * request.CardCount;
                var totalPrize = request.Prizes.Sum();

                _logger.LogInformation($"刮刮樂購買: MemberId={memberId}, CardValue={request.CardValue}, CardCount={request.CardCount}, TotalCost={totalCost}, TotalPrize={totalPrize}");

                // 檢查餘額
                if (member.Points < totalCost)
                {
                    return Json(new { success = false, message = "餘額不足" });
                }

                var pointsBefore = member.Points;
                var pointsAfter = member.Points - totalCost + totalPrize;
                var totalBetAfter = member.TotalBet + totalCost;
                var totalWinAfter = member.TotalWin + totalPrize;

                // 使用 SQL 直接更新會員點數
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE members SET points = {0}, total_bet = {1}, total_win = {2} WHERE id = {3}",
                    pointsAfter,
                    totalBetAfter,
                    totalWinAfter,
                    memberId.Value
                );

                // 保存投注記錄
                var betRecord = new BetRecord
                {
                    MemberId = memberId.Value,
                    GameType = "刮刮樂",
                    IssueNo = DateTime.UtcNow.Ticks,
                    BetNumbers = JsonSerializer.Serialize(new { CardValue = request.CardValue, CardCount = request.CardCount }),
                    BetAmount = totalCost,
                    WinningNumbers = JsonSerializer.Serialize(request.Prizes),
                    WinAmount = totalPrize,
                    PointsBefore = pointsBefore,
                    PointsAfter = pointsAfter,
                    Result = totalPrize > 0 ? $"中獎 {totalPrize} 元" : "未中獎",
                    CreatedAt = DateTime.UtcNow
                };

                _context.BetRecords.Add(betRecord);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"刮刮樂記錄已保存: TotalCost={totalCost}, TotalPrize={totalPrize}, PointsAfter={pointsAfter}");

                return Json(new
                {
                    success = true,
                    newBalance = pointsAfter,
                    totalPrize = totalPrize,
                    debug = new { pointsBefore, totalCost, totalPrize, pointsAfter }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刮刮樂購買錯誤");
                return Json(new { success = false, message = ex.Message, error = ex.ToString() });
            }
        }
    }

    // 刮刮樂購買請求模型
    public class ScratchPurchaseRequest
    {
        public int CardValue { get; set; }
        public int CardCount { get; set; }
        public List<int> Prizes { get; set; } = new List<int>();
    }

    // 輔助方法類
    public static class BetHelper
    {
        // 生成不重複的隨機數
        public static List<int> GenerateUniqueNumbers(int count, int min, int max)
        {
            var random = new Random();
            var numbers = new HashSet<int>();
            while (numbers.Count < count)
            {
                numbers.Add(random.Next(min, max + 1));
            }
            return numbers.ToList();
        }

        // 計算威力彩中獎金額
        public static decimal CalculatePowerWin(List<int> betFirst, List<int> betSecond, List<int> drawFirst, List<int> drawSecond, decimal betAmount)
        {
            var firstMatches = betFirst.Count(drawFirst.Contains);
            var secondMatches = betSecond.Count(drawSecond.Contains);

            // 簡化的中獎規則
            if (firstMatches == 6 && secondMatches == 1) return betAmount * 50000000m; // 頭獎
            if (firstMatches == 6) return betAmount * 2000000m; // 貳獎
            if (firstMatches == 5 && secondMatches == 1) return betAmount * 100000m; // 參獎
            if (firstMatches == 5) return betAmount * 10000m; // 肆獎
            if (firstMatches == 4 && secondMatches == 1) return betAmount * 1000m; // 伍獎
            if (firstMatches == 4) return betAmount * 200m; // 陸獎
            if (firstMatches == 3 && secondMatches == 1) return betAmount * 50m; // 柒獎
            if (firstMatches == 3) return betAmount * 10m; // 捌獎
            if (firstMatches == 2 && secondMatches == 1) return betAmount * 5m; // 玖獎
            return 0;
        }

        // 計算雙色球中獎金額
        public static decimal CalculateDoubleColorWin(List<int> betRed, List<int> betBlue, List<int> drawRed, List<int> drawBlue, decimal betAmount)
        {
            var redMatches = betRed.Count(drawRed.Contains);
            var blueMatches = betBlue.Count(drawBlue.Contains);

            // 簡化的中獎規則
            if (redMatches == 6 && blueMatches == 1) return betAmount * 5000000m; // 一等獎
            if (redMatches == 6) return betAmount * 200000m; // 二等獎
            if (redMatches == 5 && blueMatches == 1) return betAmount * 3000m; // 三等獎
            if (redMatches == 5 || (redMatches == 4 && blueMatches == 1)) return betAmount * 200m; // 四等獎
            if (redMatches == 4 || (redMatches == 3 && blueMatches == 1)) return betAmount * 10m; // 五等獎
            if ((redMatches == 2 && blueMatches == 1) || (redMatches == 1 && blueMatches == 1) || blueMatches == 1) return betAmount * 5m; // 六等獎
            return 0;
        }
    }
}
