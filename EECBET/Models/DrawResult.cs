using System.Collections.Generic;

namespace EECBET.Models
{
    public class DrawResult
    {
        public int IssueNo { get; set; }           // 期數
        public List<int> DrawNumbers { get; set; } = new List<int>(); // 開獎號碼
        public List<int> Numbers { get; set; } = new List<int>();      // 玩家號碼
        public int MatchingCount { get; set; }     // 相符數量
        public List<WinDetail> Wins { get; set; } = new List<WinDetail>();
        public int TotalPayout { get; set; }       // 總派彩
    }
}

