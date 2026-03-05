using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Repositories.Base;

namespace WebApiNet.Infrastructure.Repositories.Auth
{
    public interface IAuthRepository : IRepository<Cliente, string>
    {
        Task<Cliente> GetByEmailAsync(string email);
    }
}
