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
        //�ŧi GetGameListAsync()
        //Task<T> = �u�@�ӷ|�^�ǵ��G���D�P�B�u�@�v
        private async Task<List<GameListViewModel>> GetGameListAsync()
        {
            //async �����n�f�t await �ϥΡA�D�P�B���@�k�A�t�Υi�H�b���� API ���P�ɡA�h�A�Ȩ�L�ϥΪ�
            var response = await _httpClient.GetAsync("api/gamelist");

            if (response.IsSuccessStatusCode)
            {
                //Ū�� API �^�Ǥ��e���নC#���榡
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
