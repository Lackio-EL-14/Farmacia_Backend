using Farmacia.Application.Services;
using Farmacia.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly VentaService _ventaService;

        public VentaController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaDto dto)
        {
            try
            {
                var venta = await _ventaService.RegistrarVenta(dto);
                return Ok(new
                {
                    mensaje = "Venta registrada correctamente",
                    venta.Id,
                    venta.Total
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("anular/{id}")]
        public async Task<IActionResult> AnularVenta(int id)
        {
            try
            {
                await _ventaService.AnularVenta(id);
                return Ok(new { mensaje = "Venta anulada y stock restaurado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
