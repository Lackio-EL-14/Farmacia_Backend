using Farmacia.Core.Interfaces;
using Farmacia.Core.Entities;
using Farmacia.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Farmacia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork; // Para buscar al usuario
        private readonly IPasswordService _passwordService;

        public TokenController(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authentication([FromBody] UserLoginDto login)
        {
            // 1. Validar si existe el usuario (buscamos por NombreUsuario o Email)
            // Nota: Aquí uso GetAll() y filtro en memoria por rapidez, 
            // lo ideal sería tener un método GetByUsername en tu repositorio.
            var usuarios = await _unitOfWork.Usuarios.GetAll();
            var user = usuarios.FirstOrDefault(x =>
                (x.NombreUsuario == login.Usuario || x.Email == login.Usuario) && x.Activo);

            if (user == null) return NotFound("Usuario no encontrado o inactivo.");

            // 2. Verificar Contraseña (Hash vs Texto Plano)
            var isValid = _passwordService.Check(user.PasswordHash, login.Password);

            if (!isValid) return Unauthorized("Contraseña incorrecta.");

            // 3. Generar Token
            var token = GenerateToken(user);

            return Ok(new { token });
        }

        // Método privado para crear el JWT
        private string GenerateToken(Usuario user)
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!)
            );
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256
            );
            var header = new JwtHeader(signingCredentials);

            // Claims (Datos que van DENTRO del token)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.NombreUsuario),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RolId.ToString()), // O el nombre del rol si lo tienes cargado
                new Claim("Id", user.Id.ToString())
            };

            // Payload
            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(120) // Token dura 2 horas
            );

            // Generar Token
            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("migrar-passwords")]
        public async Task<IActionResult> MigrarPasswords()
        {
            // 1. Traer todos los usuarios
            var usuarios = await _unitOfWork.Usuarios.GetAll();
            int count = 0;

            foreach (var user in usuarios)
            {
                // 2. Verificar si la contraseña YA es un hash (contiene puntos)
                // Si no tiene puntos, asumimos que es texto plano antigua (ej: "123456")
                if (!user.PasswordHash.Contains("."))
                {
                    // 3. Hasheamos la contraseña vieja
                    var nuevoHash = _passwordService.Hash(user.PasswordHash);

                    // 4. Reemplazamos
                    user.PasswordHash = nuevoHash;

                    // 5. Guardamos en BD
                    await _unitOfWork.Usuarios.Update(user);
                    count++;
                }
            }

            await _unitOfWork.SaveChangesAsync(); // Commit de todos los cambios

            return Ok($"Migración completada. Se actualizaron {count} usuarios al formato Hash seguro.");
        }
    }


}