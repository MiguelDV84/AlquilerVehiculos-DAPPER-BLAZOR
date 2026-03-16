using Dapper;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.FacturaRepo;
using WebApiNet.Shared.Paged;

namespace WebApiNet.Infrastructure.Repositories.FacturaRepository
{
    public class FacturaRepository : IFacturaRepository
    {

        private readonly DapperContext _context;

        public FacturaRepository(DapperContext context)
        {
            _context = context;
        }


        public async Task<Factura> AddAsync(Factura entity)
        {
            using var connection = _context.CreateReadConnection();

            string procedureName = "sp_insertar_factura";
            var parameters = new DynamicParameters();

            parameters.Add("@p_numero_factura", entity.NumeroFactura);
            parameters.Add("@p_fecha_emision", entity.FechaEmision);
            parameters.Add("@p_fecha_vencimiento", entity.FechaVencimiento);
            parameters.Add("@p_base_imponible", entity.BaseImponible);
            parameters.Add("@p_porcentaje_iva", entity.PorcentajeIVA);
            parameters.Add("@p_total", entity.Total);
            parameters.Add("@p_estado", entity.Estado);
            parameters.Add("@p_observaciones", entity.Observaciones);
            parameters.Add("@p_cliente_id", entity.ClienteId);
            parameters.Add("@p_alquiler_id", entity.AlquilerId);

            var result = await connection.QueryFirstOrDefaultAsync<Factura>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateReadConnection();

            string procedureName = "sp_eliminar_factura";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            int filasAfectadas = await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return filasAfectadas > 0;
        }

        public async Task<PagedResult<Factura>> GetAllAsync(int pageNumber, int pageSize)
        {
            using var connection = _context.CreateReadConnection();

            string procedureName = "sp_obtener_facturas";
            var parameters = new DynamicParameters();
            parameters.Add("@p_page_number", pageNumber);
            parameters.Add("@p_page_size", pageSize);

            var result = await connection.QueryMultipleAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int totalCount = await result.ReadFirstAsync<int>();
            var items = (await result.ReadAsync<Factura>()).ToList();

            return new PagedResult<Factura>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Factura?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateReadConnection();

            string procedureName = "sp_obtener_factura_por_id";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            var result = await connection.QueryFirstOrDefaultAsync<Factura>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<Factura> UpdateAsync(int id, Factura entity)
        {
            using var connection = _context.CreateReadConnection();

            string procedureName = "sp_actualizar_factura";
            var parameters = new DynamicParameters();

            parameters.Add("@p_id", id);
            parameters.Add("@p_numero_factura", entity.NumeroFactura);
            parameters.Add("@p_fecha_emision", entity.FechaEmision);
            parameters.Add("@p_fecha_vencimiento", entity.FechaVencimiento);
            parameters.Add("@p_base_imponible", entity.BaseImponible);
            parameters.Add("@p_porcentaje_iva", entity.PorcentajeIVA);
            parameters.Add("@p_total", entity.Total);
            parameters.Add("@p_estado", entity.Estado);
            parameters.Add("@p_observaciones", entity.Observaciones);
            parameters.Add("@p_cliente_id", entity.ClienteId);
            parameters.Add("@p_alquiler_id", entity.AlquilerId);

            var result = await connection.QueryFirstOrDefaultAsync<Factura>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
