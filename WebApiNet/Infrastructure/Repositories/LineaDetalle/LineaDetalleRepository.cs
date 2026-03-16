using Dapper;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.ILineaDetalle;
using WebApiNet.Shared.Paged;

namespace WebApiNet.Infrastructure.Repositories.LineaDetalleRepository
{
    public class LineaDetalleRepository : ILineaDetalleRepository
    {
        private readonly DapperContext _context;

        public LineaDetalleRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<LineaDetalle> AddAsync(LineaDetalle entity)
        {
            var connection = _context.GetWriteConnection();

            string procedureName = "sp_insert_linea_detalle";
            var parameters = new DynamicParameters();

            parameters.Add("@p_factura_id", entity.FacturaId);
            parameters.Add("@p_alquiler_id", entity.AlquilerId);
            parameters.Add("@p_descripcion", entity.Descripcion);
            parameters.Add("@p_cantidad", entity.Cantidad);
            parameters.Add("@p_precio_unitario", entity.PrecioUnitario);
            parameters.Add("@p_subtotal", entity.Subtotal);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var connection = _context.GetWriteConnection();

            string procedureName = "sp_delete_linea_detalle";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            int filasAfectadas = await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return filasAfectadas > 0;
        }

        public async Task<PagedResult<LineaDetalle>> GetAllAsync(int pageNumber, int pageSize)
        {
            var connection = _context.CreateReadConnection();

            string procedureName = "sp_obtener_lineas_detalle";
            var parameters = new DynamicParameters();
            parameters.Add("@p_page_number", pageNumber);
            parameters.Add("@p_page_size", pageSize);

            using (connection)
            {
                var result = await connection.QueryMultipleAsync(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int totalCount = await result.ReadFirstAsync<int>();
                var items = (await result.ReadAsync<LineaDetalle>()).ToList();

                return new PagedResult<LineaDetalle>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<LineaDetalle?> GetByIdAsync(int id)
        {
            var connection = _context.CreateReadConnection();

            string procedureName = "sp_obtener_linea_detalle";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            using (connection)
            {
                var result = await connection.QueryFirstOrDefaultAsync<LineaDetalle>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<LineaDetalle> UpdateAsync(int id, LineaDetalle entity)
        {
            var connection = _context.GetWriteConnection();

            string procedureName = "sp_actualizar_linea_detalle";
            var parameters = new DynamicParameters();

            parameters.Add("@p_id", id);
            parameters.Add("@p_factura_id", entity.FacturaId);
            parameters.Add("@p_alquiler_id", entity.AlquilerId);
            parameters.Add("@p_descripcion", entity.Descripcion);
            parameters.Add("@p_cantidad", entity.Cantidad);
            parameters.Add("@p_precio_unitario", entity.PrecioUnitario);
            parameters.Add("@p_subtotal", entity.Subtotal);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return entity;
        }
    }
}