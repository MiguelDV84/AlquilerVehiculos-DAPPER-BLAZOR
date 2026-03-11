using Dapper;
using MySqlConnector;
using System.Data;
using WebApiNet.Infrastructure.Data.TypeHandlers;

namespace WebApiNet.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }

        public IDbConnection CreateConnection()
            => new MySqlConnection(_connectionString);
    }
}
