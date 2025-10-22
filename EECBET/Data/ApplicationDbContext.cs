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

        // 點數管理系統的資料表
        public DbSet<Transaction> Transactions { get; set; }

        // 如果你有其他資料表，也在這裡加入
        // 例如：
        // public DbSet<User> Users { get; set; }
        // public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 可選：設定資料表的額外配置
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PointsChange).HasColumnType("decimal(10,2)");
            });
        }
    }
}