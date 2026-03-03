using Dapper;
using System.Data;
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Presentation.Paged;

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

        public async Task<bool> DeleteAsync(string id)
        {
            using var connection = _context.CreateConnection();
            string procedureName = "sp_delete_cliente";
            var parameters = new DynamicParameters();
            parameters.Add("@p_dni", id);

            int filasAfectadas = await connection.ExecuteAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
                );

            return filasAfectadas > 0;
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

        public async Task<PagedResult<Cliente>> GetPagedAsync(int pageNumber, int pageSize)
        {
            using var connection = _context.CreateConnection();
            string procedureName = "sp_obtener_clientes";
            var parameters = new DynamicParameters();
            parameters.Add("@p_page_number", pageNumber);
            parameters.Add("@p_page_size", pageSize);

            using (var result = await connection.QueryMultipleAsync(
                procedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            ))
            {
                // 1. Leemos el primer SELECT (el COUNT)
                int totalCount = await result.ReadFirstAsync<int>();

                // 2. Leemos el segundo SELECT (los DATOS)
                var clientes = (await result.ReadAsync<Cliente>()).ToList();

                return new PagedResult<Cliente>
                {
                    Items = clientes,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }
    }
}
