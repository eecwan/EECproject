namespace WebApplication1.Models
{
    public class WinDetail
    {
        public int Type { get; set; }              // 二星、三星、四星
        public int WinningGroups { get; set; }     // 中獎組數
        public int PayoutPerGroup { get; set; }    // 每組賠率
        public int TotalPayout { get; set; }       // 總派彩
    }
}
