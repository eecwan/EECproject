using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EECBET.Models
{
  [Table("bet_records")]
  public class BetRecord
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("member_id")]
    public int MemberId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("game_type")]
    public string GameType { get; set; } = string.Empty;  // 遊戲類型：539, 大樂透, 威力彩等

    [Required]
    [Column("issue_no")]
    public long IssueNo { get; set; }

    [Column("bet_numbers")]
    public string? BetNumbers { get; set; }  // JSON格式儲存投注號碼

    [Column("bet_amount", TypeName = "numeric(18,2)")]
    public decimal BetAmount { get; set; }

    [Column("winning_numbers")]
    public string? WinningNumbers { get; set; }  // JSON格式儲存開獎號碼

    [Column("win_amount", TypeName = "numeric(18,2)")]
    public decimal WinAmount { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("result")]
    public string? Result { get; set; }  // 結果描述

    [Column("points_before", TypeName = "numeric(18,2)")]
    public decimal PointsBefore { get; set; }

    [Column("points_after", TypeName = "numeric(18,2)")]
    public decimal PointsAfter { get; set; }
  }
}

