using Microsoft.EntityFrameworkCore;
using ValeraApi.Models;

namespace ValeraApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Valera> Valeras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Valera>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Health).IsRequired();
                entity.Property(e => e.Alcohol).IsRequired();
                entity.Property(e => e.Joy).IsRequired();
                entity.Property(e => e.Fatigue).IsRequired();
                entity.Property(e => e.Money).IsRequired().HasColumnType("decimal(18,2)");
            });
        }
    }
}