using Microsoft.EntityFrameworkCore;
using EECBET.Models;

namespace EECBET.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<GameListViewModel> GameList { get; set; }
        public DbSet<SlotRecord> SlotRecords { get; set; }
        public DbSet<BetRecord> BetRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設定 Members 資料表的索引
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Username)
                .IsUnique();

            // 設定 BetRecord 的配置
            modelBuilder.Entity<BetRecord>(entity =>
            {
                entity.ToTable("bet_records");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.MemberId).HasColumnName("member_id").IsRequired();
                entity.Property(e => e.GameType).HasColumnName("game_type").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IssueNo).HasColumnName("issue_no").IsRequired();
                entity.Property(e => e.BetNumbers).HasColumnName("bet_numbers").HasColumnType("text");
                entity.Property(e => e.BetAmount).HasColumnName("bet_amount").HasColumnType("numeric(18,2)");
                entity.Property(e => e.WinningNumbers).HasColumnName("winning_numbers").HasColumnType("text");
                entity.Property(e => e.WinAmount).HasColumnName("win_amount").HasColumnType("numeric(18,2)");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.Result).HasColumnName("result").HasColumnType("text");
                entity.Property(e => e.PointsBefore).HasColumnName("points_before").HasColumnType("numeric(18,2)");
                entity.Property(e => e.PointsAfter).HasColumnName("points_after").HasColumnType("numeric(18,2)");
            });
        }
    }
}
