using Dapper;
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

        public Task<Cliente> AddAsync(Cliente entity)
        {
            throw new NotImplementedException();
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

        public Task<Cliente> UpdateAsync(string id, Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
