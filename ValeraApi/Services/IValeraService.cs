using ValeraApi.Models;
using ValeraApi.DTOs;

namespace ValeraApi.Services
{
    public interface IValeraService
    {
        Task<List<ValeraDto>> GetAllValerasAsync(int alcohol);
        Task<ValeraDto?> GetValeraByIdAsync(int id);
        Task<ValeraDto> CreateValeraAsync(CreateValeraDto createValeraDto);
        Task<ValeraDto?> UpdateValeraAsync(int id, UpdateValeraDto updateValeraDto);
        Task<bool> DeleteValeraAsync(int id);
        Task<ValeraDto?> ExecuteActionAsync(int id, string action);
    }
}