using Farmacia.Application.Services;
using Farmacia.Core.Entities;
using Farmacia.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class KardexController : ControllerBase
    {
        private readonly KardexService _service;

        public KardexController(KardexService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene el historial de movimientos (entradas/salidas) de un producto.
        /// </summary>
        /// <param name="productoId">ID del producto a consultar.</param>
        /// <param name="pageNumber">Número de página (default 1).</param>
        /// <param name="pageSize">Cantidad de registros por página (default 10).</param>
        [HttpGet("producto/{productoId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MovimientoInventario>>), 200)]
        public async Task<IActionResult> GetKardexPorProducto(int productoId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var kardex = await _service.GetKardexByProducto(productoId, pageNumber, pageSize);

            var metadata = kardex.GetMetadata();

            var response = new ApiResponse<IEnumerable<MovimientoInventario>>(
                kardex.Items,
                true,
                "Historial de movimientos recuperado",
                metadata
            );

            return Ok(response);
        }
    }
}