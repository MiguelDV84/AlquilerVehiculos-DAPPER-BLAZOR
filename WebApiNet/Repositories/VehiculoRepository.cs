using Dapper;
using System.Data;
using WebApiNet.Models;

namespace WebApiNet.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly IDbConnection _db;

        public VehiculoRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<Vehiculos>> GetAllAsync()
        {
            string procedureName = "sp_obtener_vehiculos";

            var result = await _db.QueryAsync<Vehiculos>(
                procedureName,
                commandType: CommandType.StoredProcedure
                );

            return result.ToList().AsReadOnly();
        }

        public async Task<Vehiculos?> GetByIdAsync(string matricula)
        {
            string procedureName = "sp_obtener_vehiculo_por_matricula";
            var parameters = new DynamicParameters();
            parameters.Add("@matricula_vehiculo", matricula);

            var result = await _db.QueryFirstOrDefaultAsync<Vehiculos>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<Vehiculos> AddAsync(Vehiculos vehiculo)
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

        public async Task<Vehiculos> UpdateAsync(Vehiculos vehiculo)
        {
            string procedureName = "sp_update_vehiculo";
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

    }
}
