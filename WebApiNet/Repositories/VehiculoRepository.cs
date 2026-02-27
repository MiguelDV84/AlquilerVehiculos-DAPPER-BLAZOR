using Dapper;
using System.Data;
using WebApiNet.Models;

namespace WebApiNet.Repositories
{
    public class VehiculoRepository
    {
        private readonly IDbConnection _db;

        public VehiculoRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Vehiculos>> GetAllAsync()
        {
            string procedureName = "sp_obtener_vehiculos";

            return await _db.QueryAsync<Vehiculos>(
                procedureName,
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<Vehiculos?> GetByIdAsync(string matricula)
        {
            string procedureName = "sp_obtener_vehiculo_por_matricula";
            var parameters = new DynamicParameters();
            parameters.Add("@matricula_vehiculo", matricula);

            var result =  await _db.QueryFirstOrDefaultAsync<Vehiculos>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return result;
        }
    }
}
