using Dapper;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Presentation.Paged;

namespace WebApiNet.Infrastructure.Repositories.Vehiculos
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly DapperContext _context;

        public VehiculoRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<Vehiculo?> GetByIdAsync(string matricula)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_obtener_vehiculo_por_matricula";
            var parameters = new DynamicParameters();
            parameters.Add("@matricula_vehiculo", matricula);

            var result = await connection.QueryFirstOrDefaultAsync<Vehiculo>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<Vehiculo> AddAsync(Vehiculo vehiculo)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_insertar_vehiculo";
            var parameters = new DynamicParameters();
            parameters.Add("@p_matricula", vehiculo.Matricula);
            parameters.Add("@p_tipo_vehiculo", vehiculo.TipoVehiculo);
            parameters.Add("@p_kilometraje", vehiculo.Kilometraje);
            parameters.Add("@p_marca", vehiculo.Marca);
            parameters.Add("@p_modelo", vehiculo.Modelo);
            parameters.Add("@p_precio", vehiculo.Precio);
            parameters.Add("@p_litros_tanque", vehiculo.LitrosTanque);
            parameters.Add("@p_estado", vehiculo.Estado);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return vehiculo;
        }

        public async Task<bool> DeleteAsync(string matricula)
        {
            using var connection = _context.CreateConnection();


            string procedureName = "sp_delete_vehiculo";
            var parameters = new DynamicParameters();
            parameters.Add("@p_matricula", matricula);

            int filasAfectadas = await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );
            return filasAfectadas > 0 ? true : false;
        }

        public async Task<Vehiculo> UpdateAsync(string matricula, Vehiculo vehiculo)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_update_vehiculo";
            var parameters = new DynamicParameters();

            parameters.Add("@p_matricula", matricula);
            parameters.Add("@p_tipo_vehiculo", vehiculo.TipoVehiculo);
            parameters.Add("@p_kilometraje", vehiculo.Kilometraje);
            parameters.Add("@p_marca", vehiculo.Marca);
            parameters.Add("@p_modelo", vehiculo.Modelo);
            parameters.Add("@p_precio", vehiculo.Precio);
            parameters.Add("@p_litros_tanque", vehiculo.LitrosTanque);
            parameters.Add("@p_estado", vehiculo.Estado);

            await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return vehiculo;
        }

        public async Task<PagedResult<Vehiculo>> GetAllAsync(int pageNumber, int pageSize)
        {
            using var connection = _context.CreateConnection();
            string procedureName = "sp_obtener_vehiculos";
            var parameters = new DynamicParameters();
            parameters.Add("@p_page_number", pageNumber);
            parameters.Add("@p_page_size", pageSize);

            var result = await connection.QueryMultipleAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            int totalCount = await result.ReadFirstAsync<int>();
            var items = (await result.ReadAsync<Vehiculo>()).ToList();

           return new PagedResult<Vehiculo>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
