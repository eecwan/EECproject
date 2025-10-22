using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EECBET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EECBET.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PointsController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public PointsController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: api/points/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
      try
      {
        var transactions = await _context.Transactions.ToListAsync();

        var summary = new TransactionSummary
        {
          TotalIncome = transactions
                .Where(t => t.PointsChange > 0)
                .Sum(t => t.PointsChange),

          TotalExpense = transactions
                .Where(t => t.PointsChange < 0)
                .Sum(t => Math.Abs(t.PointsChange)),

          TotalBalance = transactions.Sum(t => t.PointsChange),

          TodayChange = transactions
                .Where(t => t.TransactionTime.Date == DateTime.Today)
                .Sum(t => t.PointsChange)
        };

        return Ok(new
        {
          success = true,
          data = new
          {
            totalBalance = summary.TotalBalance,
            todayChange = summary.TodayChange,
            totalIncome = summary.TotalIncome,
            totalExpense = summary.TotalExpense
          }
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          success = false,
          error = ex.Message
        });
      }
    }

    // GET: api/points/transactions
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string category)
    {
      try
      {
        var query = _context.Transactions.AsQueryable();

        if (startDate.HasValue)
        {
          query = query.Where(t => t.TransactionTime.Date >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
          query = query.Where(t => t.TransactionTime.Date <= endDate.Value.Date);
        }

        if (!string.IsNullOrEmpty(category))
        {
          query = query.Where(t => t.Category == category);
        }

        var transactions = await query
            .OrderByDescending(t => t.TransactionTime)
            .Select(t => new
            {
              id = t.Id,
              transactionTime = t.TransactionTime,
              category = t.Category,
              amount = t.Amount,
              type = t.Type,
              pointsChange = t.PointsChange,
              status = t.Status
            })
            .ToListAsync();

        return Ok(new
        {
          success = true,
          data = transactions
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new
        {
          success = false,
          error = ex.Message
        });
      }
    }
  }
}