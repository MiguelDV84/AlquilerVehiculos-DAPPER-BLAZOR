using Dapper;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Shared.Paged;

namespace WebApiNet.Infrastructure.Repositories.AlquilerRepo
{
    public class AlquilerRepository : IAlquilerRepository
    {

        private readonly DapperContext _context;

        public AlquilerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Alquiler> AddAsync(Alquiler alquiler)
        {
            using var connection = _context.CreateConnection();
            
            string procedureName = "sp_insert_alquiler";
            var parameters = new DynamicParameters();
            parameters.Add("@p_fecha_alquiler", alquiler.FechaAlquiler);
            parameters.Add("@p_fecha_devolucion_prevista", alquiler.FechaDevolucionPrevista);
            parameters.Add("@p_fecha_devolucion_real", alquiler.FechaDevolucionReal);
            parameters.Add("@p_precio", alquiler.Precio);
            parameters.Add("@p_dni_cliente", alquiler.ClienteDni);
            parameters.Add("@p_matricula_vehiculo", alquiler.VehiculoMatricula);

            await connection.ExecuteAsync(
                procedureName, 
                parameters, 
                commandType: CommandType.StoredProcedure
                );

            return alquiler;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            
            string procedureName = "sp_delete_alquiler";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            int filasAfectadas = await connection.ExecuteAsync(
                procedureName, 
                parameters, 
                commandType: CommandType.StoredProcedure
                );

            return filasAfectadas > 0 ? true : false;
        }

        public Task<IReadOnlyList<Alquiler>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Alquiler>> GetAllAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Alquiler?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_obtener_alquiler_id";
            var parameters = new DynamicParameters();
            parameters.Add("@p_id", id);

            var alquiler = await connection.QueryFirstOrDefaultAsync<Alquiler>(
                procedureName, 
                parameters, 
                commandType: CommandType.StoredProcedure
                );

            return alquiler;
        }

        public async Task<PagedResult<Alquiler>> GetPagedAsync(int pageNumber, int pageSize)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_obtener_alquileres";
            var parameters = new DynamicParameters();

            parameters.Add("@p_page_number", pageNumber);
            parameters.Add("@p_page_size", pageSize);

            using var result = await connection.QueryMultipleAsync(
                procedureName, 
                parameters, 
                commandType: CommandType.StoredProcedure
                );

            int totalCount = await result.ReadFirstAsync<int>();

            var alquileres = (await result.ReadAsync<Alquiler>()).ToList();

            return new PagedResult<Alquiler>
            {
                Items = alquileres,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Alquiler> UpdateAsync(int id, Alquiler entity)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_update_alquiler";
            var parameters = new DynamicParameters();

            parameters.Add("@p_id", id);
            parameters.Add("@p_fecha_alquiler", entity.FechaAlquiler);
            parameters.Add("@p_fecha_devolucion_prevista", entity.FechaDevolucionPrevista);
            parameters.Add("@p_precio", entity.Precio);

            await connection.ExecuteAsync(
                procedureName, 
                parameters, 
                commandType: CommandType.StoredProcedure
                );

            return entity;
        }
    }
}
