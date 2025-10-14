using Microsoft.AspNetCore.Mvc;

namespace EECBET.Controllers
{
    public class SlotMachineController : Controller
    {
        // GET: /SlotMachine/Index
        public IActionResult Index()
        {
            return View();
        }

        // 如果未來需要處理遊戲邏輯，可以在這裡添加
        // 例如：儲存遊戲分數、更新使用者點數等
    }
}