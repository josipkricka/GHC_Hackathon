using Microsoft.EntityFrameworkCore;
using my_api_project.Models;

namespace my_api_project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CardHolder> CardHolders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransJobType> TransJobTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardHolder>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.CardHolder)
                .HasForeignKey(t => t.CardHolderId);

            modelBuilder.Entity<TransJobType>()
                .HasMany(j => j.Transactions)
                .WithOne(t => t.TransJobType)
                .HasForeignKey(t => t.TransJobTypeId);
        }
    }
} 	