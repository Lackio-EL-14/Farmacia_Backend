
using Dapper;
using Farmacia.Core.DTOs;
using Farmacia.Core.Pagination;
using Farmacia.Infrastructure.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VentaDapperRepository : IVentaDapperRepository
{
    private readonly string _connString;
    public VentaDapperRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection");
    }
    private IDbConnection CreateConnection() => new SqlConnection(_connString);

    public async Task<PagedList<VentaDtos>> GetPagedAsync(Farmacia.Core.DTOs.VentaFilterDto filter)
    {
        using var conn = CreateConnection();
        var sbWhere = new StringBuilder(" WHERE 1 = 1 ");
        var parameters = new DynamicParameters();

        if (filter.ClienteId.HasValue)
        {
            sbWhere.Append(" AND v.ClienteId = @ClienteId ");
            parameters.Add("ClienteId", filter.ClienteId.Value);
        }
        if (filter.FechaDesde.HasValue)
        {
            sbWhere.Append(" AND v.Fecha >= @FechaDesde ");
            parameters.Add("FechaDesde", filter.FechaDesde.Value);
        }
        if (filter.FechaHasta.HasValue)
        {
            sbWhere.Append(" AND v.Fecha <= @FechaHasta ");
            parameters.Add("FechaHasta", filter.FechaHasta.Value);
        }
        if (!string.IsNullOrWhiteSpace(filter.NumeroFactura))
        {
            sbWhere.Append(" AND v.NumeroFactura LIKE '%' + @NumeroFactura + '%' ");
            parameters.Add("NumeroFactura", filter.NumeroFactura);
        }

        // total count
        var sqlCount = $"SELECT COUNT(1) FROM Ventas v {sbWhere}";
        var total = await conn.ExecuteScalarAsync<int>(sqlCount, parameters);

        // pagination
        var page = filter.Page ?? 1;
        var pageSize = filter.PageSize ?? 10;
        var skip = (page - 1) * pageSize;
        parameters.Add("skip", skip);
        parameters.Add("take", pageSize);

        var sql = $@"
            SELECT v.Id, v.Fecha, v.Total, v.ClienteId, v.NumeroFactura
            FROM Ventas v
            {sbWhere}
            ORDER BY v.Fecha DESC, v.Id DESC
            OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

        var items = (await conn.QueryAsync<VentaDtos>(sql, parameters)).ToList();

        return new PagedList<VentaDtos>(items, total, page, pageSize);
    }

    public async Task<VentaDtos?> GetByIdAsync(int id)
    {
        using var conn = CreateConnection();
        var sql = @"SELECT Id, Fecha, Total, ClienteId, NumeroFactura FROM Ventas WHERE Id = @Id";
        return await conn.QueryFirstOrDefaultAsync<VentaDtos>(sql, new { Id = id });
    }
}
