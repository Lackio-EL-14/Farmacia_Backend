using Farmacia.Core.Interfaces;
using Farmacia.Core.Pagination;
using Farmacia.Core.Response;
using Farmacia.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Farmacia.Infrastructure.DTOs;


namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        /// <summary>
        /// Obtiene todas las facturas registradas con soporte de filtros y paginación.
        /// </summary>
        /// <param name="filter">Parámetros opcionales de filtrado y paginación.</param>
        /// <returns>Lista paginada de facturas.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedList<FacturaDto>>), 200)]
        public async Task<IActionResult> GetFacturas([FromQuery] FacturaFilterDto filter)
        {
            // Fix: Map FacturaDtos to FacturaDto before creating ApiResponse
            var facturas = await _facturaService.GetFacturasAsync(filter);

            // Convert PagedList<FacturaDtos> to PagedList<FacturaDto>
            var mappedItems = facturas.Items.Select(f => new FacturaDto
            {
                Id = f.Id,
                NumeroFactura = f.Numero != null ? int.Parse(f.Numero) : 0,
                NumeroAutorizacion = "", // Set appropriately if available
                FechaEmision = f.FechaEmision,
                VentaId = f.VentaId
            }).ToList();

            var mappedPagedList = PagedList<FacturaDto>.Create(
                mappedItems,
                facturas.CurrentPage,
                facturas.PageSize
            );

            var response = new ApiResponse<PagedList<FacturaDto>>(
                mappedPagedList,
                true,
                "Facturas obtenidas correctamente",
                mappedPagedList.GetMetadata()
            );

            return Ok(response);
        }

        /// <summary>
        /// Obtiene una factura por su identificador.
        /// </summary>
        /// <param name="id">ID de la factura.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<FacturaDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFacturaById(int id)
        {
            var factura = await _facturaService.GetFacturaByIdAsync(id);
            if (factura == null)
                return NotFound(new ApiResponse<string>(null, false, $"No se encontró la factura con ID {id}", null));

            // Map FacturaDtos to FacturaDto
            var mappedFactura = new FacturaDto
            {
                Id = factura.Id,
                NumeroFactura = factura.Numero != null ? int.Parse(factura.Numero) : 0,
                NumeroAutorizacion = "", // Set appropriately if available
                FechaEmision = factura.FechaEmision,
                VentaId = factura.VentaId
            };

            return Ok(new ApiResponse<FacturaDto>(mappedFactura, true, "Factura encontrada correctamente", null));
        }

        /// <summary>
        /// Crea una nueva factura asociada a una venta existente.
        /// </summary>
        /// <param name="dto">Datos de la factura.</param>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<FacturaDto>), 201)]
        public async Task<IActionResult> CreateFactura([FromBody] FacturaDto dto)
        {
            // Map FacturaDto to FacturaDtos before passing to service
            var facturaToCreate = new FacturaDtos
            {
                Id = dto.Id,
                Numero = dto.NumeroFactura.ToString(),
                FechaEmision = dto.FechaEmision,
                VentaId = dto.VentaId
                // Map other properties if needed
            };

            var facturaCreada = await _facturaService.CreateFacturaAsync(facturaToCreate);

            // Map FacturaDtos to FacturaDto for response
            var mappedFactura = new FacturaDto
            {
                Id = facturaCreada.Id,
                NumeroFactura = facturaCreada.Numero != null ? int.Parse(facturaCreada.Numero) : 0,
                NumeroAutorizacion = "", // Set appropriately if available
                FechaEmision = facturaCreada.FechaEmision,
                VentaId = facturaCreada.VentaId
            };

            var response = new ApiResponse<FacturaDto>(mappedFactura, true, "Factura creada correctamente", null);
            return CreatedAtAction(nameof(GetFacturaById), new { id = mappedFactura.Id }, response);
        }

        /// <summary>
        /// Elimina una factura (opcionalmente, marcar como anulada).
        /// </summary>
        /// <param name="id">ID de la factura.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            await _facturaService.DeleteFacturaAsync(id);
            return Ok(new ApiResponse<string>(null, true, "Factura eliminada correctamente", null));
        }
    }
}
