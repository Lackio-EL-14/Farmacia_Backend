using AutoMapper;
using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Farmacia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Recupera todos los clientes registrados.
        /// </summary>
        /// <remarks>
        /// Este metodo obtiene una lista de todos los clientes almacenados en la base de datos y los devuelve en formato DTO.
        /// </remarks>
        /// <param>   </param>
        /// <returns></returns>
        /// <responsecode="200">Retorna todos los clientes</responsecode>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
        {
            var clientes = await _unitOfWork.Clientes.GetAll();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }
        /*
        [HttpGet]

        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                return NotFound();
            return Ok(_mapper.Map<ClienteDto>(cliente));
        }
        */
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ClienteDto dto)
        {
            var entity = _mapper.Map<Cliente>(dto);
            await _unitOfWork.Clientes.Add(entity);
            return Ok(new { message = "Cliente creado correctamente" });
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Update(int id, [FromBody] ClienteDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = _mapper.Map<Cliente>(dto);
            await _unitOfWork.Clientes.Update(entity);
            return Ok(new { message = "Cliente actualizado correctamente" });
        } 

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetById(id);
            if (cliente == null) return NotFound();
            await _unitOfWork.Clientes.Delete(id);
            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }

}
