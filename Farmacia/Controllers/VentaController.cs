using Farmacia.Application.Services;
using Farmacia.Core.Exceptions;
using Farmacia.Core.Response;
using Farmacia.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly VentaService _ventaService;
        private IVentaDapperRepository @object;

        public VentaController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        public VentaController(IVentaDapperRepository @object)
        {
            this.@object = @object;
        }

        /// <summary>
        /// Registra una venta completa, actualiza stock y genera factura electrónica.
        /// </summary>
        [HttpPost]
        [SwaggerResponse(200, "Venta registrada", typeof(ApiResponse<VentaDtos>))]
        [SwaggerResponse(400, "Error en datos")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaDto dto)
        {
            if (dto == null) throw new ApiException("Datos de venta inválidos", 400);
            var venta = await _ventaService.RegistrarVenta(dto);
            var resp = new ApiResponse<VentaDtos>(venta, true, "Venta registrada correctamente.");
            return Ok(resp);
        }

        /// <summary>
        /// Anula una venta y restaura el stock.
        /// </summary>
        [HttpPost("{id}/anular")]
        [SwaggerResponse(200, "Venta anulada")]
        public async Task<IActionResult> AnularVenta(int id)
        {
            await _ventaService.AnularVenta(id);
            return Ok(new ApiResponse<object>(null, true, "Venta anulada y stock restaurado correctamente."));
        }
    }
}
