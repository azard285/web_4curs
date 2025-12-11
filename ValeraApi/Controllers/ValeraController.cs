// ValeraApi/Controllers/ValeraController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValeraApi.Services;
using ValeraApi.DTOs;
using System.Security.Claims;

namespace ValeraApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Все методы требуют аутентификации
    public class ValeraController : ControllerBase
    {
        private readonly IValeraService _valeraService;

        public ValeraController(IValeraService valeraService)
        {
            _valeraService = valeraService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Только админ может видеть всех
        public async Task<ActionResult<List<ValeraDto>>> GetAllValeras(int alcohol)
        {
            var valeras = await _valeraService.GetAllValerasAsync(alcohol);
            return Ok(valeras);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<ValeraDto>>> GetMyValeras(int alcohol)
        {
            var userId = GetCurrentUserId();
            var valeras = await _valeraService.GetMyValerasAsync(userId, alcohol);
            return Ok(valeras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValeraDto>> GetValeraById(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            
            var valera = await _valeraService.GetValeraByIdAsync(id, userId, isAdmin);
            if (valera == null)
            {
                return NotFound($"Valera with id {id} not found or access denied");
            }
            return Ok(valera);
        }

        [HttpPost]
        public async Task<ActionResult<ValeraDto>> CreateValera(CreateValeraDto createValeraDto)
        {
            var userId = GetCurrentUserId();
            var valera = await _valeraService.CreateValeraAsync(createValeraDto, userId);
            return CreatedAtAction(nameof(GetValeraById), new { id = valera.Id }, valera);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ValeraDto>> UpdateValera(int id, UpdateValeraDto updateValeraDto)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            
            var valera = await _valeraService.UpdateValeraAsync(id, updateValeraDto, userId, isAdmin);
            if (valera == null)
            {
                return NotFound($"Valera with id {id} not found or access denied");
            }
            return Ok(valera);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteValera(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            
            var result = await _valeraService.DeleteValeraAsync(id, userId, isAdmin);
            if (!result)
            {
                return NotFound($"Valera with id {id} not found or access denied");
            }
            return NoContent();
        }

        [HttpPost("{id}/actions")]
        public async Task<ActionResult<ValeraDto>> ExecuteAction(int id, [FromBody] string action)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            
            var valera = await _valeraService.ExecuteActionAsync(id, action, userId, isAdmin);
            if (valera == null)
            {
                return NotFound($"Valera with id {id} not found, action '{action}' is invalid, or access denied");
            }
            return Ok(valera);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }
    }
}