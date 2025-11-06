using System;
using System.ComponentModel.DataAnnotations;
namespace EECBET.Models
{
    //Data Annotations（屬性標籤）/簡單寫法/適合欄位不多的資料表/[Key]、[Column]、[Required]
    public class GameListViewModel
    {

        [Key] //主鍵（必要）
        public int GameID { get; set; }
        public string? GameNameTW { get; set; }
        public string? GameNameEN { get; set; }
        public string? GameCode { get; set; }
        public string? GameCategory { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsActive { get; set; }
        public string? GameImageUrl { get; set; }
        public string? GameLink { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
