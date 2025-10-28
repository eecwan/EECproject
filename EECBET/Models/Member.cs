using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EECBET.Models
{
    [Table("members")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ 確保 PostgreSQL 自動遞增
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("email")]
        public string? Email { get; set; }

        [StringLength(50)]
        [Column("firstname")]
        public string? Firstname { get; set; }

        [StringLength(50)]
        [Column("lastname")]
        public string? Lastname { get; set; }

        [StringLength(10)]
        [Column("gender")]
        public string? Gender { get; set; }

        // ✅ 改用 DateOnly，因為 Neon 的型別是 date
        [Column("birthday", TypeName = "date")]
        public DateOnly? Birthday { get; set; }

        [StringLength(100)]
        [Column("country")]
        public string? Country { get; set; }

        [Column("points", TypeName = "numeric(18,2)")]
        public decimal Points { get; set; } = 0;

        // ✅ 改成 DateOnly，用 UTC 時間轉換為今天的日期
        [Column("created_at", TypeName = "date")]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        // ✅ 改用 DateOnly，因為 Neon 的型別是 date
        [Column("last_login", TypeName = "date")]
        public DateOnly? LastLogin { get; set; }

        [Column("total_bet", TypeName = "numeric(18,2)")]
        public decimal TotalBet { get; set; } = 0;

        [Column("total_win", TypeName = "numeric(18,2)")]
        public decimal TotalWin { get; set; } = 0;
    }
}
