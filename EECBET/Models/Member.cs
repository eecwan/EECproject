using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EECBET.Models
{
    [Table("members")]
    public class Member
    {
        [Key]
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

        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        [StringLength(100)]
        [Column("country")]
        public string? Country { get; set; }

        [Column("points", TypeName = "decimal(18,2)")]
        public decimal Points { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("total_bet", TypeName = "decimal(18,2)")]
        public decimal TotalBet { get; set; } = 0;

        [Column("total_win", TypeName = "decimal(18,2)")]
        public decimal TotalWin { get; set; } = 0;
    }
}
