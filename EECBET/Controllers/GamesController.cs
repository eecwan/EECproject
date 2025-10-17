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

        // �� HttpClient �P Logger �X�ְ_��, ILogger<GamesController> �u�O�@�Ӭ�����
        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
            // Web API ��m
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5210/") 
            };
        }

        // �q Web API ���o�C���M��
        private async Task<List<GameListViewModel>> GetGameListAsync()
        {
            var response = await _httpClient.GetAsync("api/gamelist");

            if (response.IsSuccessStatusCode)
            {
                var games = await response.Content.ReadFromJsonAsync<List<GameListViewModel>>();
                return games ?? new List<GameListViewModel>();
            }

            // �p�G�s�u���ѡA�^�ǪŪ� List�]�קK�{�������^
            return new List<GameListViewModel>();
        }


        //    SlotGame ����     
        public async Task<IActionResult> SlotGame_Exclusive()
        {
            var games = await GetGameListAsync();//�� API ���
            return View(games);   //���ƶǵ� SlotGame_Exclusive.cshtml
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


        //���~�B�z
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
