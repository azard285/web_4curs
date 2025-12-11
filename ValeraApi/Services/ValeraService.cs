// ValeraApi/Services/ValeraService.cs
using Microsoft.EntityFrameworkCore;
using ValeraApi.Data;
using ValeraApi.Models;
using ValeraApi.DTOs;

namespace ValeraApi.Services
{
    public class ValeraService : IValeraService
    {
        private readonly AppDbContext _context;

        public ValeraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ValeraDto>> GetAllValerasAsync(int alcohol)
        {
            var valeras = await _context.Valeras
                .Where(v => v.Alcohol <= alcohol)
                .Include(v => v.User) // Включаем пользователя
                .ToListAsync();

            return valeras.Select(v => MapToDto(v)).ToList();
        }

        public async Task<List<ValeraDto>> GetMyValerasAsync(int userId, int alcohol)
        {
            var valeras = await _context.Valeras
                .Where(v => v.UserId == userId && v.Alcohol <= alcohol)
                .ToListAsync();

            return valeras.Select(v => MapToDto(v)).ToList();
        }

        public async Task<ValeraDto?> GetValeraByIdAsync(int id, int userId, bool isAdmin)
        {
            var valera = await _context.Valeras
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (valera == null) return null;
            
            // Проверка прав доступа
            if (!isAdmin && valera.UserId != userId) return null;

            return MapToDto(valera);
        }

        public async Task<ValeraDto> CreateValeraAsync(CreateValeraDto createValeraDto, int userId)
        {
            var valera = new Valera(
                createValeraDto.Health,
                createValeraDto.Alcohol,
                createValeraDto.Joy,
                createValeraDto.Fatigue,
                createValeraDto.Money
            )
            {
                UserId = userId // Привязываем к пользователю
            };

            _context.Valeras.Add(valera);
            await _context.SaveChangesAsync();

            return MapToDto(valera);
        }

        public async Task<ValeraDto?> UpdateValeraAsync(int id, UpdateValeraDto updateValeraDto, int userId, bool isAdmin)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null) return null;

            // Проверка прав доступа
            if (!isAdmin && valera.UserId != userId) return null;

            var updatedValera = new Valera(
                updateValeraDto.Health,
                updateValeraDto.Alcohol,
                updateValeraDto.Joy,
                updateValeraDto.Fatigue,
                updateValeraDto.Money
            )
            {
                Id = id,
                UserId = valera.UserId // Сохраняем связь с пользователем
            };

            _context.Entry(valera).CurrentValues.SetValues(updatedValera);
            await _context.SaveChangesAsync();

            return MapToDto(updatedValera);
        }

        public async Task<bool> DeleteValeraAsync(int id, int userId, bool isAdmin)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null) return false;

            // Проверка прав доступа
            if (!isAdmin && valera.UserId != userId) return false;

            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ValeraDto?> ExecuteActionAsync(int id, string action, int userId, bool isAdmin)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null) return null;

            // Проверка прав доступа
            if (!isAdmin && valera.UserId != userId) return null;

            bool success = true;

            switch (action.ToLower())
            {
                case "gotowork":
                    success = valera.GoToWork();
                    break;
                case "contemplatenature":
                    valera.ContemplateNature();
                    break;
                case "drinkwineandwatchseries":
                    valera.DrinkWineAndWatchSeries();
                    break;
                case "gotobar":
                    valera.GoToBar();
                    break;
                case "drinkwithmarginals":
                    valera.DrinkWithMarginals();
                    break;
                case "singinmetro":
                    valera.SingInMetro();
                    break;
                case "sleep":
                    valera.Sleep();
                    break;
                default:
                    return null;
            }

            if (!success && action.ToLower() == "gotowork")
            {
                return null;
            }

            await _context.SaveChangesAsync();

            return MapToDto(valera);
        }

        private ValeraDto MapToDto(Valera valera)
        {
            return new ValeraDto
            {
                Id = valera.Id,
                Health = valera.Health,
                Alcohol = valera.Alcohol,
                Joy = valera.Joy,
                Fatigue = valera.Fatigue,
                Money = valera.Money,
                UserId = valera.UserId // Добавляем UserId в DTO
            };
        }
    }
}