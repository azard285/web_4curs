using ValeraApi.Models;

namespace ValeraApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже админ
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    Email = "admin@valera.com",
                    Username = "Admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}