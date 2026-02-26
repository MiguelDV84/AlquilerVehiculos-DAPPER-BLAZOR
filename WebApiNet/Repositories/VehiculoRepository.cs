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
            const string sql = "SELECT * FROM Vehiculos";
            return await _db.QueryAsync<Vehiculos>(sql);
        }
    }
}
