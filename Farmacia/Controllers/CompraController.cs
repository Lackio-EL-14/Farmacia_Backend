using Farmacia.Application.Services;
using Farmacia.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _service;

        public CompraController(CompraService service)
        {
            _service = service;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCompra(RegistrarCompraDto dto)
        {
            var id = await _service.RegistrarCompraAsync(dto);

            return Ok(new
            {
                message = "Compra registrada exitosamente",
                data = new { compraId = id }
            });
        }
    }
}
