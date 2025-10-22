using System.Diagnostics;
using EECBET.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json; // for GetFromJsonAsync / PostAsJsonAsync
using System.Threading.Tasks;


namespace EECBET.Controllers
{
    public class GamesController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<GamesController> _logger;

        // 把 HttpClient 與 Logger 合併起來, ILogger<GamesController> 只是一個紀錄用
        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
            // Web API 位置
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5210/") 
            };
        }

        // 從 Web API 取得遊戲清單
        //宣告 GetGameListAsync()
        //Task<T> = 「一個會回傳結果的非同步工作」
        private async Task<List<GameListViewModel>> GetGameListAsync()
        {
            //async 必須要搭配 await 使用，非同步的作法，系統可以在等待 API 的同時，去服務其他使用者
            var response = await _httpClient.GetAsync("api/gamelist");

            if (response.IsSuccessStatusCode)
            {
                //讀取 API 回傳內容並轉成C#的格式
                var games = await response.Content.ReadFromJsonAsync<List<GameListViewModel>>();
                return games ?? new List<GameListViewModel>();
            }

            // 如果連線失敗，回傳空的 List（避免程式報錯）
            return new List<GameListViewModel>();
        }


        //    SlotGame 分頁     
        public async Task<IActionResult> SlotGame_Exclusive()
        {
            var games = await GetGameListAsync();//撈 API 資料
            return View(games);   //把資料傳給 SlotGame_Exclusive.cshtml
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


        //錯誤處理
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
