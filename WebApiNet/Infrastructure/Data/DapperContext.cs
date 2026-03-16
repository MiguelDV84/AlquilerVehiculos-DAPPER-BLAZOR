using Dapper;
using MySqlConnector;
using System.Data;
using WebApiNet.Infrastructure.Data.TypeHandlers;

namespace WebApiNet.Infrastructure.Data
{
    public class DapperContext : IDisposable
    {
        private readonly string _connectionString;

        private IDbConnection? _writeConnection;
        private IDbTransaction? _transaction;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }

        public IDbConnection CreateReadConnection()
            => new MySqlConnection(_connectionString);

        public IDbConnection GetWriteConnection()
        {
            if (_writeConnection is not null) return _writeConnection;

            _writeConnection = new MySqlConnection(_connectionString);
            _writeConnection.Open();
            return _writeConnection;
        }

        public void Commit()
        {
            _transaction?.Commit();
            Reset();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            Reset();
        }

        private void Reset()
        {
            _transaction?.Dispose();
            _writeConnection?.Dispose();
            _transaction = null;
            _writeConnection = null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
