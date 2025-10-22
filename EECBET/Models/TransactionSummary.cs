namespace EECBET.Models
{
  public class TransactionSummary
  {
    public decimal TotalIncome { get; set; }      // 總中獎額（盈）
    public decimal TotalExpense { get; set; }     // 總投注額（虧）
    public decimal TotalBalance { get; set; }     // 點數總額
    public decimal TodayChange { get; set; }      // 今日盈虧
  }
}