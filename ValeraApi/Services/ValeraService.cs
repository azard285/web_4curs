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

        public async Task<List<ValeraDto>> GetAllValerasAsync()
        {
            var valeras = await _context.Valeras.ToListAsync();
            return valeras.Select(v => MapToDto(v)).ToList();
        }

        public async Task<ValeraDto?> GetValeraByIdAsync(int id)
        {
            var valera = await _context.Valeras.FindAsync(id);
            return valera == null ? null : MapToDto(valera);
        }

        public async Task<ValeraDto> CreateValeraAsync(CreateValeraDto createValeraDto)
        {
            var valera = new Valera(
                createValeraDto.Health,
                createValeraDto.Alcohol,
                createValeraDto.Joy,
                createValeraDto.Fatigue,
                createValeraDto.Money
            );

            _context.Valeras.Add(valera);
            await _context.SaveChangesAsync();

            return MapToDto(valera);
        }

        public async Task<ValeraDto?> UpdateValeraAsync(int id, UpdateValeraDto updateValeraDto)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null) return null;

            // Создаем нового Валеру с обновленными значениями
            var updatedValera = new Valera(
                updateValeraDto.Health,
                updateValeraDto.Alcohol,
                updateValeraDto.Joy,
                updateValeraDto.Fatigue,
                updateValeraDto.Money
            )
            {
                Id = id // Сохраняем тот же ID
            };

            // Обновляем сущность в контексте
            _context.Entry(valera).CurrentValues.SetValues(updatedValera);
            await _context.SaveChangesAsync();

            return MapToDto(updatedValera);
        }

        public async Task<bool> DeleteValeraAsync(int id)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null) return false;

            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ValeraDto?> ExecuteActionAsync(int id, string action)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null) return null;

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
                return null; // Не удалось пойти на работу
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
                Money = valera.Money
            };
        }
    }
}