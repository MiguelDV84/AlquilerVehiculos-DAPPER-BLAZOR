using Dapper;
using MySqlX.XDevAPI;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;

namespace WebApiNet.Infrastructure.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        public readonly DapperContext _context;

        public AuthRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Cliente> UpdateAsync(string id, Cliente cliente)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_update_cliente";
            var parameters = new DynamicParameters();
            parameters.Add("@p_dni", cliente.Dni);
            parameters.Add("@p_email", cliente.Email);
            parameters.Add("@p_password_hash", cliente.PasswordHash);
            parameters.Add("@p_nombre", cliente.Nombre);


            var result = await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return cliente;
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_insertar_cliente";
            var parameters = new DynamicParameters();
            parameters.Add("@p_dni", cliente.Dni);
            parameters.Add("@p_email", cliente.Email);
            parameters.Add("@p_password_hash", cliente.PasswordHash);
            parameters.Add("@p_nombre", cliente.Nombre);
            parameters.Add("@p_role", cliente.Role);

            var result = await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return cliente;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Cliente>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Cliente?> GetByIdAsync(string id)
        {
            using var connection = _context.CreateConnection();

            string procedureName = "sp_obtener_cliente_dni";
            var parameters = new DynamicParameters();
            parameters.Add("@p_email_cliente", id);

            var result = await connection.QueryFirstOrDefaultAsync<Cliente>(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return result;
        }


    }
}
