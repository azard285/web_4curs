using Microsoft.AspNetCore.Mvc;
using ValeraApi.Services;
using ValeraApi.DTOs;

namespace ValeraApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValeraController : ControllerBase
    {
        private readonly IValeraService _valeraService;

        public ValeraController(IValeraService valeraService)
        {
            _valeraService = valeraService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ValeraDto>>> GetAllValeras(int alcohol)
        {
            var valeras = await _valeraService.GetAllValerasAsync( alcohol);
            return Ok(valeras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ValeraDto>> GetValeraById(int id)
        {
            var valera = await _valeraService.GetValeraByIdAsync(id);
            if (valera == null)
            {
                return NotFound($"Valera with id {id} not found");
            }
            return Ok(valera);
        }

        [HttpPost]
        public async Task<ActionResult<ValeraDto>> CreateValera(CreateValeraDto createValeraDto)
        {
            var valera = await _valeraService.CreateValeraAsync(createValeraDto);
            return CreatedAtAction(nameof(GetValeraById), new { id = valera.Id }, valera);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ValeraDto>> UpdateValera(int id, UpdateValeraDto updateValeraDto)
        {
            var valera = await _valeraService.UpdateValeraAsync(id, updateValeraDto);
            if (valera == null)
            {
                return NotFound($"Valera with id {id} not found");
            }
            return Ok(valera);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteValera(int id)
        {
            var result = await _valeraService.DeleteValeraAsync(id);
            if (!result)
            {
                return NotFound($"Valera with id {id} not found");
            }
            return NoContent();
        }

       [HttpPost("{id}/actions")]
        public async Task<ActionResult<ValeraDto>> ExecuteAction(int id, [FromBody] string action)
        {
            var valera = await _valeraService.ExecuteActionAsync(id, action);
            if (valera == null)
            {
                return NotFound($"Valera with id {id} not found or action '{action}' is invalid");
            }
            return Ok(valera);
        }   
    }
}