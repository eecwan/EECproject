// 檔案路徑: EECBET/Models/SlotRecord.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace EECBET.Models
{
    public class SlotRecord
    {
        [Key]
        public int Id { get; set; }

        public DateTime PlayTime { get; set; } = DateTime.Now;

        [Required]
        public int Bet { get; set; }

        [Required]
        public string Result { get; set; } = "";

        [Required]
        public int Reward { get; set; }

        [Required]
        public int CreditsAfter { get; set; }
    }
}
