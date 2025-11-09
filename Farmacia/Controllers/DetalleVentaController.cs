using Farmacia.Core.DTOs;
using Farmacia.Core.Interfaces;
using Farmacia.Core.Pagination;
using Farmacia.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleVentaController : ControllerBase
    {
        private readonly IDetalleVentaService _detalleVentaService;

        public DetalleVentaController(IDetalleVentaService detalleVentaService)
        {
            _detalleVentaService = detalleVentaService;
        }

        /// <summary>
        /// Obtiene todos los detalles de venta con soporte de filtros y paginación.
        /// </summary>
        /// <param name="filter">Parámetros opcionales de filtrado y paginación.</param>
        /// <returns>Lista paginada de detalles de venta.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedList<DetalleVentaDto>>), 200)]
        public async Task<IActionResult> GetDetalles([FromQuery] DetalleVentaFilterDto filter)
        {
            var detalles = await _detalleVentaService.GetDetallesAsync(filter);
            var response = new ApiResponse<PagedList<DetalleVentaDto>>(
                detalles,
                "Detalles de venta obtenidos correctamente",
                detalles.GetMetadata()
            );

            return Ok(response);
        }

        /// <summary>
        /// Obtiene los detalles de una venta específica.
        /// </summary>
        /// <param name="ventaId">ID de la venta.</param>
        [HttpGet("venta/{ventaId}")]
        [ProducesResponseType(typeof(ApiResponse<List<DetalleVentaDto>>), 200)]
        public async Task<IActionResult> GetDetallesByVentaId(int ventaId)
        {
            var detalles = await _detalleVentaService.GetByVentaIdAsync(ventaId);
            var response = new ApiResponse<List<DetalleVentaDto>>(detalles, "Detalles de venta obtenidos correctamente");
            return Ok(response);
        }

        /// <summary>
        /// Crea un nuevo detalle de venta asociado a una venta.
        /// </summary>
        /// <param name="dto">Datos del detalle de venta.</param>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<DetalleVentaDto>), 201)]
        public async Task<IActionResult> CreateDetalle([FromBody] DetalleVentaDto dto)
        {
            var detalle = await _detalleVentaService.CreateDetalleAsync(dto);
            var response = new ApiResponse<DetalleVentaDto>(detalle, "Detalle de venta creado correctamente");
            return CreatedAtAction(nameof(GetDetallesByVentaId), new { ventaId = detalle.VentaId }, response);
        }

        /// <summary>
        /// Elimina un detalle de venta por su ID.
        /// </summary>
        /// <param name="id">ID del detalle.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            await _detalleVentaService.DeleteDetalleAsync(id);
            return Ok(new ApiResponse<string>(data: null, message: "Detalle de venta eliminado correctamente"));
        }
    }
}
