namespace EECBET.Models
{
    public class GameListViewModel
    {
        public int GameID { get; set; }
        public string GameNameTW { get; set; }
        public string GameNameEN { get; set; }
        public string GameCode { get; set; }
        public string GameCategory { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsActive { get; set; }
    }
}
