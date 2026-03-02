using Dapper;
using System.Data;
using WebApiNet.Core.Entities;

namespace WebApiNet.Infrastructure.Repositories.Vehiculos
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly IDbConnection _db;

        public VehiculoRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<Vehiculo>> GetAllAsync()
        {
            string procedureName = "sp_obtener_vehiculos";

            var result = await _db.QueryAsync<Vehiculo>(
                procedureName,
                commandType: CommandType.StoredProcedure
                );

            return result.ToList().AsReadOnly();
        }

        public async Task<Vehiculo?> GetByIdAsync(string matricula)
        {
            string procedureName = "sp_obtener_vehiculo_por_matricula";
            var parameters = new DynamicParameters();
            parameters.Add("@matricula_vehiculo", matricula);

            var result = await _db.QueryFirstOrDefaultAsync<Vehiculo>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<Vehiculo> AddAsync(Vehiculo vehiculo)
        {
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

            await _db.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return vehiculo;
        }

        public async Task<bool> DeleteAsync(string matricula)
        {
            string procedureName = "sp_delete_vehiculo";
            var parameters = new DynamicParameters();
            parameters.Add("@p_matricula", matricula);

            int filasAfectadas = await _db.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );
            return filasAfectadas > 0 ? true : false;
        }

        public async Task<Vehiculo> UpdateAsync(string matricula, Vehiculo vehiculo)
        {
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

            await _db.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return vehiculo;
        }

    }
}
