// ValeraApi/Services/IValeraService.cs
using ValeraApi.DTOs;

namespace ValeraApi.Services
{
    public interface IValeraService
    {
        Task<List<ValeraDto>> GetAllValerasAsync(int alcohol);
        Task<List<ValeraDto>> GetMyValerasAsync(int userId, int alcohol);
        Task<ValeraDto?> GetValeraByIdAsync(int id, int userId, bool isAdmin);
        Task<ValeraDto> CreateValeraAsync(CreateValeraDto createValeraDto, int userId);
        Task<ValeraDto?> UpdateValeraAsync(int id, UpdateValeraDto updateValeraDto, int userId, bool isAdmin);
        Task<bool> DeleteValeraAsync(int id, int userId, bool isAdmin);
        Task<ValeraDto?> ExecuteActionAsync(int id, string action, int userId, bool isAdmin);
    }
}