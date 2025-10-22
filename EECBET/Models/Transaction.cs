using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EECBET.Models
{
  [Table("transactions")]
  public class Transaction
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("transaction_time")]
    public DateTime TransactionTime { get; set; }

    [Required]
    [StringLength(100)]
    [Column("category")]
    public string Category { get; set; }

    [Required]
    [Column("amount", TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(20)]
    [Column("type")]
    public string Type { get; set; } // 未中獎, 中獎, 處理中

    [Required]
    [Column("points_change", TypeName = "decimal(10,2)")]
    public decimal PointsChange { get; set; }

    [StringLength(20)]
    [Column("status")]
    public string Status { get; set; } = "已結單"; // 已結單, 待處理

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
  }
}