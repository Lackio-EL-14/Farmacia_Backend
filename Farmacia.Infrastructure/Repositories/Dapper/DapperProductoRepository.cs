
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient; // o System.Data.SqlClient
using Microsoft.Extensions.Configuration;
using Farmacia.Core.DTOs;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.DTOs; // si tienes DTOs

namespace Farmacia.Infrastructure.Repositories.Dapper
{
    public interface IDapperProductoRepository
    {
        Task<IEnumerable<ProductoDto>> GetAllAsync(ProductFilterDto filter);
        Task<ProductoDto?> GetByIdAsync(int id);
    }

    public class DapperProductoRepository : IDapperProductoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connString;
        public DapperProductoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connString);

        public async Task<IEnumerable<ProductoDto>> GetAllAsync(ProductFilterDto filter)
        {
            using var conn = CreateConnection();
            var sql = @"SELECT p.Id, p.Nombre, p.Precio, p.Stock, p.CategoriaId, p.Activo
                        FROM Productos p
                        WHERE (@Search IS NULL OR p.Nombre LIKE '%' + @Search + '%')
                          AND (@CategoriaId IS NULL OR p.CategoriaId = @CategoriaId)
                          AND (@Activo IS NULL OR p.Activo = @Activo)";
            var result = await conn.QueryAsync<ProductoDto>(sql, new
            {
                Search = string.IsNullOrWhiteSpace(filter.Search) ? null : filter.Search,
                CategoriaId = filter.CategoriaId,
                Activo = filter.Activo
            });

            // Aplicar paginación en memoria o preferible con SQL con OFFSET/FETCH
            if (filter.Page.HasValue)
            {
                var skip = (filter.Page.Value - 1) * (filter.PageSize ?? 10);
                result = result.Skip(skip).Take(filter.PageSize ?? 10);
            }

            return result;
        }

        public async Task<ProductoDto?> GetByIdAsync(int id)
        {
            using var conn = CreateConnection();
            var sql = "SELECT Id, Nombre, Precio, Stock, CategoriaId, Activo FROM Productos WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<ProductoDto>(sql, new { Id = id });
        }
    }
}
