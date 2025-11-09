
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Farmacia.Core.DTOs;
using Farmacia.Core.Pagination;
using System.Linq;
using System.Text;

public class DetalleVentaDapperRepository : IDetalleVentaDapperRepository
{
    private readonly string _connString;
    public DetalleVentaDapperRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection");
    }
    private IDbConnection CreateConnection() => new SqlConnection(_connString);

    public async Task<IEnumerable<DetalleVentaDto>> GetByVentaIdAsync(int ventaId)
    {
        using var conn = CreateConnection();
        var sql = @"
            SELECT dv.Id, dv.VentaId, dv.ProductoId, p.Nombre AS ProductoNombre, dv.Cantidad, dv.PrecioUnitario
            FROM DetalleVenta dv
            INNER JOIN Productos p ON p.Id = dv.ProductoId
            WHERE dv.VentaId = @VentaId";
        return await conn.QueryAsync<DetalleVentaDto>(sql, new { VentaId = ventaId });
    }

    public async Task<PagedList<DetalleVentaDto>> GetPagedAsync(DetalleVentaFilterDto filter)
    {
        using var conn = CreateConnection();
        var sbWhere = new StringBuilder(" WHERE 1 = 1 ");
        var parameters = new DynamicParameters();

        if (filter.VentaId.HasValue)
        {
            sbWhere.Append(" AND dv.VentaId = @VentaId ");
            parameters.Add("VentaId", filter.VentaId.Value);
        }
        if (filter.ProductoId.HasValue)
        {
            sbWhere.Append(" AND dv.ProductoId = @ProductoId ");
            parameters.Add("ProductoId", filter.ProductoId.Value);
        }

        var sqlCount = $"SELECT COUNT(1) FROM DetalleVenta dv {sbWhere}";
        var total = await conn.ExecuteScalarAsync<int>(sqlCount, parameters);

        var page = filter.Page ?? 1;
        var pageSize = filter.PageSize ?? 10;
        var skip = (page - 1) * pageSize;
        parameters.Add("skip", skip);
        parameters.Add("take", pageSize);

        var sql = $@"
            SELECT dv.Id, dv.VentaId, dv.ProductoId, p.Nombre AS ProductoNombre, dv.Cantidad, dv.PrecioUnitario
            FROM DetalleVenta dv
            LEFT JOIN Productos p ON p.Id = dv.ProductoId
            {sbWhere}
            ORDER BY dv.Id DESC
            OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

        var items = (await conn.QueryAsync<DetalleVentaDto>(sql, parameters)).ToList();
        return new PagedList<DetalleVentaDto>(items, total, page, pageSize);
    }
}
