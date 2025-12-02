using Farmacia.Application.Services;
using Farmacia.Infrastructure.DTOs;
using Farmacia.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CajaController : ControllerBase
    {
        private readonly CajaService _service;

        public CajaController(CajaService service)
        {
            _service = service;
        }

        [HttpPost("abrir")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> AbrirCaja([FromBody] AperturaCajaDto dto)
        {
            await _service.AbrirCajaAsync(dto);
            return Ok(new ApiResponse<string>("Caja habilitada", true, "Operación exitosa"));
        }

        [HttpPost("cerrar")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> CerrarCaja([FromBody] CierreCajaDto dto)
        {
            var arqueo = await _service.CerrarCajaAsync(dto);

            // Resumen para mostrar al usuario si le faltó plata
            var resumen = new
            {
                FechaCierre = arqueo.Fecha,
                VentasRegistradas = arqueo.Ingresos,
                DineroReportado = dto.DineroFisicoEnCaja,
                Diferencia = arqueo.Diferencia, // Negativo = Faltante, Positivo = Sobrante
                EstadoCaja = "CERRADA"
            };

            return Ok(new ApiResponse<object>(resumen, true, "Caja cerrada y arqueada correctamente"));
        }
    }
}