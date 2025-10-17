using AutoMapper;
using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            var productos = await _unitOfWork.Productos.GetAll();
            var productosDto = _mapper.Map<IEnumerable<ProductoDto>>(productos);
            return Ok(productosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var producto = await _unitOfWork.Productos.GetById(id);
            if (producto == null)
                return NotFound();

            return Ok(_mapper.Map<ProductoDto>(producto));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoDto dto)
        {
            var producto = _mapper.Map<Producto>(dto);

            await _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveChangesAsync();  

            return Ok(new { message = "Producto creado correctamente" });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductoDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var producto = _mapper.Map<Producto>(dto);
            await _unitOfWork.Productos.Update(producto);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new { message = "Producto actualizado correctamente." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _unitOfWork.Productos.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new { message = "Producto eliminado correctamente." });
        }
    }
}
