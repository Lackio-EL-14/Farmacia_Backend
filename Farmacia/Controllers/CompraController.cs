using Farmacia.Application.Services;
using Farmacia.Infrastructure.DTOs;
using Farmacia.Core.Response;
using Farmacia.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] 
    public class CompraController : ControllerBase
    {
        private readonly CompraService _service;

        public CompraController(CompraService service)
        {
            _service = service;
        }

        /// <summary>
        /// Registra una nueva compra de productos e incrementa el stock.
        /// </summary>
        /// <param name="dto">Datos de la compra y sus detalles.</param>
        /// <returns>El ID de la compra generada.</returns>
        /// <response code="200">Compra registrada exitosamente.</response>
        /// <response code="400">Error de validación o negocio (ej. Proveedor no existe).</response>
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> RegistrarCompra([FromBody] RegistrarCompraDto dto)
        {

            var id = await _service.RegistrarCompraAsync(dto);

            var response = new ApiResponse<object>(
                new { CompraId = id },
                true,
                "Compra registrada exitosamente"
            );

            return Ok(response);
        }

        /// <summary>
        /// Obtiene el listado paginado de compras realizadas.
        /// </summary>
        /// <param name="pageNumber">Número de página (default 1).</param>
        /// <param name="pageSize">Registros por página (default 10).</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Compra>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var comprasPaginadas = await _service.GetAllComprasAsync(pageNumber, pageSize);

            var metadata = comprasPaginadas.GetMetadata();

            var response = new ApiResponse<IEnumerable<Compra>>(
                comprasPaginadas.Items,
                true,
                "Listado recuperado",
                metadata
            );

            return Ok(response);
        }
    }
}