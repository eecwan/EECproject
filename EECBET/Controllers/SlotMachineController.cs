using Microsoft.AspNetCore.Mvc;
using EECBET.Data;
using EECBET.Models;
using System.Linq;

namespace EECBET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SlotController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 取得最新 50 筆紀錄
        [HttpGet]
        public IActionResult GetRecords()
        {
            var records = _context.SlotRecords
                .OrderByDescending(r => r.PlayTime)
                .Take(50)
                .ToList();

            return Ok(records);
        }

        // 儲存遊戲紀錄
        [HttpPost]
        public IActionResult SaveRecord([FromBody] SlotRecord record)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.SlotRecords.Add(record);
            _context.SaveChanges();
            return Ok(record);
        }
    }
}
