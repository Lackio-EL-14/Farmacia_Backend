
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
using Farmacia.Infrastructure.DTOs;

public class FacturaDapperRepository : IFacturaDapperRepository
{
    private readonly string _connString;
    public FacturaDapperRepository(IConfiguration configuration)
    {
        _connString = configuration.GetConnectionString("DefaultConnection");
    }
    private IDbConnection CreateConnection() => new SqlConnection(_connString);

    public async Task<PagedList<FacturaDto>> GetPagedAsync(FacturaFilterDto filter)
    {
        using var conn = CreateConnection();
        var sbWhere = new StringBuilder(" WHERE 1 = 1 ");
        var parameters = new DynamicParameters();

        if (filter.VentaId.HasValue)
        {
            sbWhere.Append(" AND f.VentaId = @VentaId ");
            parameters.Add("VentaId", filter.VentaId.Value);
        }
        if (!string.IsNullOrWhiteSpace(filter.Numero))
        {
            sbWhere.Append(" AND f.Numero LIKE '%' + @Numero + '%' ");
            parameters.Add("Numero", filter.Numero);
        }
        if (filter.FechaDesde.HasValue)
        {
            sbWhere.Append(" AND f.FechaEmision >= @FechaDesde ");
            parameters.Add("FechaDesde", filter.FechaDesde.Value);
        }
        if (filter.FechaHasta.HasValue)
        {
            sbWhere.Append(" AND f.FechaEmision <= @FechaHasta ");
            parameters.Add("FechaHasta", filter.FechaHasta.Value);
        }

        var sqlCount = $"SELECT COUNT(1) FROM Facturas f {sbWhere}";
        var total = await conn.ExecuteScalarAsync<int>(sqlCount, parameters);

        var page = filter.Page ?? 1;
        var pageSize = filter.PageSize ?? 10;
        var skip = (page - 1) * pageSize;
        parameters.Add("skip", skip);
        parameters.Add("take", pageSize);

        var sql = $@"
            SELECT f.Id, f.VentaId, f.Numero, f.FechaEmision, f.ImporteTotal
            FROM Facturas f
            {sbWhere}
            ORDER BY f.FechaEmision DESC, f.Id DESC
            OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

        var items = (await conn.QueryAsync<FacturaDtos>(sql, parameters)).ToList();
        return new PagedList<FacturaDto>(items, total, page, pageSize);
    }

    public async Task<FacturaDto?> GetByIdAsync(int id)
    {
        using var conn = CreateConnection();
        var sql = @"SELECT Id, VentaId, Numero, FechaEmision, ImporteTotal FROM Facturas WHERE Id = @Id";
        return await conn.QueryFirstOrDefaultAsync<FacturaDto>(sql, new { Id = id });
    }

    Task<PagedList<FacturaDtos>> IFacturaDapperRepository.GetPagedAsync(FacturaFilterDto filter)
    {
        throw new NotImplementedException();
    }

    Task<FacturaDtos?> IFacturaDapperRepository.GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
