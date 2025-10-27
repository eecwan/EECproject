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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設定 Members 資料表的索引
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Username)
                .IsUnique();
        }
    }
}
