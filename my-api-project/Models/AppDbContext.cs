using Microsoft.EntityFrameworkCore;
using my_api_project.Models;

namespace my_api_project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CardHolder> CardHolders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardHolder>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.CardHolder)
                .HasForeignKey(t => t.CardHolderId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId);
        }
    }
}