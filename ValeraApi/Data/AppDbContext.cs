// ValeraApi/Data/AppDbContext.cs
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
        public DbSet<User> Users { get; set; } // Добавляем Users

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка Valera
            modelBuilder.Entity<Valera>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Health).IsRequired();
                entity.Property(e => e.Alcohol).IsRequired();
                entity.Property(e => e.Joy).IsRequired();
                entity.Property(e => e.Fatigue).IsRequired();
                entity.Property(e => e.Money).IsRequired().HasColumnType("decimal(18,2)");
                
                // Связь с User
                entity.HasOne(v => v.User)
                      .WithMany(u => u.Valeras)
                      .HasForeignKey(v => v.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Настройка User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique(); // Уникальный email
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasDefaultValue("User");
            });
        }
    }
}