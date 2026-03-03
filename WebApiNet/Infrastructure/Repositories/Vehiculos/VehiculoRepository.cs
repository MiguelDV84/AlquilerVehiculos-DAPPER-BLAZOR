using Dapper;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.Paged;

namespace WebApiNet.Infrastructure.Repositories.Vehiculos
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly DapperContext _context;

        public VehiculoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Vehiculo>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_obtener_vehiculos";

            var result = await connection.QueryAsync<Vehiculo>(
                procedureName,
                commandType: CommandType.StoredProcedure
                );

            return result.ToList().AsReadOnly();
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

        public Task<PagedResult<Vehiculo>> GetPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
